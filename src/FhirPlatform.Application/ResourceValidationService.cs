using FhirPlatform.Application.Contracts;
using FhirPlatform.FhirClient;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;

namespace FhirPlatform.Application;

public interface IResourceValidationService
{
    Task<FhirValidationResponse> ValidateAsync(FhirValidationRequest request, CancellationToken cancellationToken);
}

public sealed class ResourceValidationService(IFhirResourceClient fhirClient, IExtensionRegistryReader extensionRegistry) : IResourceValidationService
{
    private readonly FhirJsonParser _jsonParser = new();
    private readonly FhirXmlParser _xmlParser = new();
    private readonly FhirJsonSerializer _jsonSerializer = new(new SerializerSettings { Pretty = true });
    private readonly FhirXmlSerializer _xmlSerializer = new(new SerializerSettings { Pretty = true });

    public async Task<FhirValidationResponse> ValidateAsync(FhirValidationRequest request, CancellationToken cancellationToken)
    {
        var resource = Parse(request);
        var errors = new List<string>();
        var warnings = new List<string>();
        var unknownExtensions = await FindUnknownExtensionsAsync(resource, cancellationToken);

        if (!string.Equals(resource.TypeName, request.ExpectedResourceType, StringComparison.OrdinalIgnoreCase))
        {
            errors.Add($"Expected {request.ExpectedResourceType} but parsed {resource.TypeName}.");
        }

        foreach (var extensionUrl in unknownExtensions)
        {
            errors.Add($"Unknown custom extension '{extensionUrl}' is not registered for {resource.TypeName}.");
        }

        var outcome = errors.Count == 0 ? await fhirClient.ValidateAsync(resource, cancellationToken) : CreateLocalOutcome(errors);
        foreach (var issue in outcome.Issue)
        {
            var message = issue.Diagnostics ?? issue.Details?.Text ?? issue.Code?.ToString() ?? "FHIR validation issue";
            if (issue.Severity is OperationOutcome.IssueSeverity.Error or OperationOutcome.IssueSeverity.Fatal) errors.Add(message);
            if (issue.Severity is OperationOutcome.IssueSeverity.Warning) warnings.Add(message);
        }

        return new FhirValidationResponse(
            errors.Count == 0,
            resource.TypeName,
            _jsonSerializer.SerializeToString(resource),
            _xmlSerializer.SerializeToString(resource),
            _jsonSerializer.SerializeToString(outcome),
            errors.Distinct(StringComparer.OrdinalIgnoreCase).ToArray(),
            warnings.Distinct(StringComparer.OrdinalIgnoreCase).ToArray(),
            unknownExtensions);
    }

    private Resource Parse(FhirValidationRequest request) => request.Format.Equals("xml", StringComparison.OrdinalIgnoreCase)
        ? _xmlParser.Parse<Resource>(request.Payload)
        : _jsonParser.Parse<Resource>(request.Payload);

    private async Task<IReadOnlyList<string>> FindUnknownExtensionsAsync(Resource resource, CancellationToken cancellationToken)
    {
        IEnumerable<Extension> resourceExtensions = resource is DomainResource domainResource
            ? domainResource.Extension
            : Enumerable.Empty<Extension>();

        if (!resourceExtensions.Any()) return Array.Empty<string>();

        var registeredUrls = await extensionRegistry.GetActiveCanonicalUrlsAsync(resource.TypeName, cancellationToken);
        return resourceExtensions
            .Select(extension => extension.Url)
            .Where(url => !string.IsNullOrWhiteSpace(url) && IsCustomExtension(url!) && !registeredUrls.Contains(url!))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray()!;
    }

    private static bool IsCustomExtension(string url) =>
        !url.StartsWith("http://hl7.org/fhir/StructureDefinition/", StringComparison.OrdinalIgnoreCase) &&
        !url.StartsWith("https://hl7.org/fhir/StructureDefinition/", StringComparison.OrdinalIgnoreCase);

    private static OperationOutcome CreateLocalOutcome(IEnumerable<string> errors)
    {
        var outcome = new OperationOutcome();
        foreach (var error in errors)
        {
            outcome.Issue.Add(new OperationOutcome.IssueComponent
            {
                Severity = OperationOutcome.IssueSeverity.Error,
                Code = OperationOutcome.IssueType.Invalid,
                Diagnostics = error
            });
        }
        return outcome;
    }
}
