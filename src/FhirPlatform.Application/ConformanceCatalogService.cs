using FhirPlatform.Application.Contracts;
using FhirPlatform.FhirClient;

namespace FhirPlatform.Application;

public interface IConformanceCatalogService
{
    Task<ConformanceDashboardDetailDto> GetDetailedDashboardAsync(CancellationToken cancellationToken);
}

public sealed class ConformanceCatalogService(IFhirResourceClient fhirClient) : IConformanceCatalogService
{
    private static readonly string[] ExpectedResources = ["Patient", "Observation", "DiagnosticReport", "Practitioner", "Organization", "Location", "ValueSet", "CodeSystem", "ConceptMap", "AuditEvent", "Provenance", "Consent"];

    public async Task<ConformanceDashboardDetailDto> GetDetailedDashboardAsync(CancellationToken cancellationToken)
    {
        var statement = await fhirClient.GetCapabilityStatementAsync(cancellationToken);
        var resources = statement.Rest.SelectMany(r => r.Resource).Select(r => new FhirResourceCapabilityDto(
            r.Type,
            r.Interaction.Select(i => i.Code?.ToString() ?? string.Empty).Where(s => s.Length > 0).ToArray(),
            r.SearchParam.Select(p => p.Name).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray()!,
            r.Operation.Select(o => o.Name).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray()!)).ToArray();
        var supported = resources.Select(r => r.ResourceType).ToHashSet(StringComparer.OrdinalIgnoreCase);
        return new ConformanceDashboardDetailDto(
            true,
            statement.FhirVersion?.ToString(),
            resources.Length,
            resources.Sum(r => r.SearchParameters.Count),
            resources.Sum(r => r.Operations.Count),
            DateTimeOffset.UtcNow,
            resources,
            ExpectedResources.Where(r => !supported.Contains(r)).ToArray());
    }
}
