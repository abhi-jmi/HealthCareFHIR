using FhirPlatform.Application.Contracts;
using FhirPlatform.FhirClient;
using Hl7.Fhir.Serialization;

namespace FhirPlatform.Application;

public interface IFhirExplorerService
{
    Task<FhirExplorerResponse> ExecuteAsync(FhirExplorerRequest request, CancellationToken cancellationToken);
}

public sealed class FhirExplorerService(IFhirResourceClient fhirClient) : IFhirExplorerService
{
    private static readonly HashSet<string> AllowedResources = new(StringComparer.OrdinalIgnoreCase)
    {
        "Patient", "Practitioner", "Organization", "Location", "Observation", "DiagnosticReport", "Condition", "AllergyIntolerance", "MedicationRequest", "Task", "Appointment", "ValueSet", "CodeSystem", "ConceptMap", "AuditEvent", "Provenance", "Consent"
    };
    private readonly FhirJsonSerializer _jsonSerializer = new(new SerializerSettings { Pretty = true });
    private readonly FhirXmlSerializer _xmlSerializer = new(new SerializerSettings { Pretty = true });

    public async Task<FhirExplorerResponse> ExecuteAsync(FhirExplorerRequest request, CancellationToken cancellationToken)
    {
        if (!AllowedResources.Contains(request.ResourceType)) throw new InvalidOperationException($"Unsupported resource type '{request.ResourceType}'.");
        var resource = request.Interaction.Equals("read", StringComparison.OrdinalIgnoreCase)
            ? await fhirClient.ReadAsync(request.ResourceType, request.Id ?? throw new InvalidOperationException("Read requires an id."), cancellationToken)
            : await fhirClient.SearchAsync(request.ResourceType, request.Parameters, cancellationToken);
        var isXml = request.Format.Equals("xml", StringComparison.OrdinalIgnoreCase);
        var body = resource is null ? string.Empty : isXml ? _xmlSerializer.SerializeToString(resource) : _jsonSerializer.SerializeToString(resource);
        return new FhirExplorerResponse(200, request.ResourceType, request.Interaction, isXml ? "application/fhir+xml" : "application/fhir+json", body, new Dictionary<string, string>());
    }
}
