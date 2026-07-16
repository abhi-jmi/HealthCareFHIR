using FhirPlatform.Application.Contracts;
using FhirPlatform.FhirClient;

namespace FhirPlatform.Application;

public interface IConformanceService { Task<CapabilityDashboardDto> GetDashboardAsync(CancellationToken cancellationToken); }

public sealed class ConformanceService(IFhirResourceClient fhirClient) : IConformanceService
{
    public async Task<CapabilityDashboardDto> GetDashboardAsync(CancellationToken cancellationToken)
    {
        var metadata = await fhirClient.GetCapabilityStatementAsync(cancellationToken);
        var resources = metadata.Rest.SelectMany(r => r.Resource).ToArray();
        return new CapabilityDashboardDto(true, metadata.FhirVersion?.ToString(), resources.Length, resources.Sum(r => r.SearchParam.Count), DateTimeOffset.UtcNow);
    }
}
