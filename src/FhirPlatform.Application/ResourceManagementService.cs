using FhirPlatform.Application.Contracts;
using FhirPlatform.FhirClient;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;

namespace FhirPlatform.Application;

public interface IResourceManagementService
{
    Task<ResourceSearchResultDto> SearchAsync(string resourceType, IReadOnlyDictionary<string, string?> parameters, CancellationToken cancellationToken);
    Task<ResourceReadResultDto> ReadAsync(string resourceType, string id, CancellationToken cancellationToken);
    Task<ResourceReadResultDto> CreateAsync(string resourceType, string payloadJson, CancellationToken cancellationToken);
    Task<ResourceReadResultDto> UpdateAsync(string resourceType, string id, string payloadJson, CancellationToken cancellationToken);
}

public sealed class ResourceManagementService(IFhirResourceClient fhirClient) : IResourceManagementService
{
    private readonly FhirJsonDeserializer _jsonDeserializer = new();

    public async Task<ResourceSearchResultDto> SearchAsync(string resourceType, IReadOnlyDictionary<string, string?> parameters, CancellationToken cancellationToken)
    {
        var bundle = await fhirClient.SearchAsync(resourceType, parameters, cancellationToken);
        return new ResourceSearchResultDto(resourceType, bundle.Total ?? bundle.Entry.Count, bundle.Entry.Select(ToSummary).ToArray(), bundle.ToJson(pretty: true));
    }

    public async Task<ResourceReadResultDto> ReadAsync(string resourceType, string id, CancellationToken cancellationToken)
    {
        var resource = await fhirClient.ReadAsync(resourceType, id, cancellationToken) ?? throw new InvalidOperationException($"{resourceType}/{id} was not found.");
        return new ResourceReadResultDto(resource.TypeName, resource.Id, resource.ToJson(pretty: true));
    }

    public async Task<ResourceReadResultDto> CreateAsync(string resourceType, string payloadJson, CancellationToken cancellationToken)
    {
        var resource = Parse(resourceType, payloadJson);
        var created = await fhirClient.CreateRawAsync(resource, cancellationToken);
        return new ResourceReadResultDto(created.TypeName, created.Id, created.ToJson(pretty: true));
    }

    public async Task<ResourceReadResultDto> UpdateAsync(string resourceType, string id, string payloadJson, CancellationToken cancellationToken)
    {
        var resource = Parse(resourceType, payloadJson);
        resource.Id = id;
        var updated = await fhirClient.UpdateRawAsync(resourceType, id, resource, cancellationToken);
        return new ResourceReadResultDto(updated.TypeName, updated.Id, updated.ToJson(pretty: true));
    }

    private Resource Parse(string resourceType, string payloadJson)
    {
        var resource = _jsonDeserializer.Deserialize<Resource>(payloadJson);
        if (!string.Equals(resource.TypeName, resourceType, StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException($"Expected {resourceType} but received {resource.TypeName}.");
        return resource;
    }

    private static ResourceSummaryDto ToSummary(Bundle.EntryComponent entry)
    {
        var resource = entry.Resource;
        var subject = resource switch
        {
            Observation x => x.Subject?.Reference,
            DiagnosticReport x => x.Subject?.Reference,
            Condition x => x.Subject?.Reference,
            AllergyIntolerance x => x.Patient?.Reference,
            MedicationRequest x => x.Subject?.Reference,
            Appointment x => string.Join(',', x.Participant.Select(p => p.Actor?.Reference).Where(x => !string.IsNullOrWhiteSpace(x))),
            Hl7.Fhir.Model.Task x => x.For?.Reference,
            _ => null
        };
        return new ResourceSummaryDto(resource.TypeName, resource.Id, Display(resource), Status(resource), Date(resource), subject);
    }

    private static string? Display(Resource resource) => resource switch
    {
        Patient x => string.Join(' ', x.Name.FirstOrDefault()?.Given ?? []).Trim() + " " + x.Name.FirstOrDefault()?.Family,
        Practitioner x => string.Join(' ', x.Name.FirstOrDefault()?.Given ?? []).Trim() + " " + x.Name.FirstOrDefault()?.Family,
        Organization x => x.Name,
        Location x => x.Name,
        Observation x => x.Code?.Text ?? x.Code?.Coding.FirstOrDefault()?.Display,
        DiagnosticReport x => x.Code?.Text ?? x.Code?.Coding.FirstOrDefault()?.Display,
        CodeSystem x => x.Title ?? x.Name,
        ValueSet x => x.Title ?? x.Name,
        _ => resource.Id
    };

    private static string? Status(Resource resource) => resource switch
    {
        Patient x => x.Active?.ToString(), Practitioner x => x.Active?.ToString(), Organization x => x.Active?.ToString(), Location x => x.Status?.ToString(),
        Observation x => x.Status?.ToString(), DiagnosticReport x => x.Status?.ToString(), MedicationRequest x => x.Status?.ToString(), Hl7.Fhir.Model.Task x => x.Status?.ToString(), Appointment x => x.Status?.ToString(), _ => null
    };

    private static string? Date(Resource resource) => resource switch
    {
        Patient x => x.BirthDate, Observation x => x.Effective?.ToString(), DiagnosticReport x => x.Effective?.ToString(), Appointment x => x.Start?.ToString("O"), _ => resource.Meta?.LastUpdated?.ToString("O")
    };
}
