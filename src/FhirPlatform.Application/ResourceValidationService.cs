using System.Collections;
using System.Reflection;
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
        var extensionUrls = CollectExtensionUrls(resource);
        if (extensionUrls.Count == 0) return Array.Empty<string>();

        var registeredUrls = await extensionRegistry.GetActiveCanonicalUrlsAsync(resource.TypeName, cancellationToken);
        return extensionUrls
            .Where(url => IsCustomExtension(url) && !registeredUrls.Contains(url))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static IReadOnlyList<string> CollectExtensionUrls(Resource resource)
    {
        var urls = new List<string>();
        var visited = new HashSet<object>(ReferenceEqualityComparer.Instance);
        Visit(resource, urls, visited);
        return urls;
    }

    private static void Visit(object? node, List<string> urls, ISet<object> visited)
    {
        if (node is null || node is string || !visited.Add(node)) return;

        if (node is Extension extension)
        {
            if (!string.IsNullOrWhiteSpace(extension.Url)) urls.Add(extension.Url!);
            foreach (var child in extension.Extension) Visit(child, urls, visited);
            Visit(extension.Value, urls, visited);
            return;
        }

        if (node is DomainResource domainResource)
        {
            foreach (var extensionNode in domainResource.Extension) Visit(extensionNode, urls, visited);
            foreach (var modifierExtension in domainResource.ModifierExtension) Visit(modifierExtension, urls, visited);
        }
        else if (node is BackboneElement backboneElement)
        {
            foreach (var extensionNode in backboneElement.Extension) Visit(extensionNode, urls, visited);
            foreach (var modifierExtension in backboneElement.ModifierExtension) Visit(modifierExtension, urls, visited);
        }
        else if (node is Element element)
        {
            foreach (var extensionNode in element.Extension) Visit(extensionNode, urls, visited);
        }

        if (node is IEnumerable enumerable)
        {
            foreach (var item in enumerable) Visit(item, urls, visited);
            return;
        }

        foreach (var property in node.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (property.GetIndexParameters().Length > 0 || property.Name is "Parent" or "Children" or "NamedChildren") continue;
            if (!typeof(Base).IsAssignableFrom(property.PropertyType) && !typeof(IEnumerable).IsAssignableFrom(property.PropertyType)) continue;
            Visit(property.GetValue(node), urls, visited);
        }
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
