using Hl7.Fhir.Model;
using Task = System.Threading.Tasks.Task;

namespace FhirPlatform.FhirClient;

public interface IFhirResourceClient
{
    Task<T?> ReadAsync<T>(string id, CancellationToken cancellationToken) where T : Resource, new();
    Task<Resource?> ReadAsync(string resourceType, string id, CancellationToken cancellationToken);
    Task<T?> VersionReadAsync<T>(string id, string versionId, CancellationToken cancellationToken) where T : Resource, new();
    Task<Bundle> SearchAsync<T>(IReadOnlyDictionary<string, string?> parameters, CancellationToken cancellationToken) where T : Resource, new();
    Task<Bundle> SearchAsync(string resourceType, IReadOnlyDictionary<string, string?> parameters, CancellationToken cancellationToken);
    Task<T> CreateAsync<T>(T resource, CancellationToken cancellationToken) where T : Resource, new();
    Task<Resource> CreateRawAsync(Resource resource, CancellationToken cancellationToken);
    Task<T> UpdateAsync<T>(string id, T resource, CancellationToken cancellationToken) where T : Resource, new();
    Task<Resource> UpdateRawAsync(string resourceType, string id, Resource resource, CancellationToken cancellationToken);
    Task<Resource> PatchAsync(string resourceType, string id, Parameters patch, CancellationToken cancellationToken);
    Task DeleteAsync<T>(string id, CancellationToken cancellationToken) where T : Resource, new();
    Task<Bundle> HistoryAsync<T>(string id, CancellationToken cancellationToken) where T : Resource, new();
    Task<Bundle> TransactionAsync(Bundle bundle, CancellationToken cancellationToken);
    Task<Bundle> BatchAsync(Bundle bundle, CancellationToken cancellationToken);
    Task<OperationOutcome> ValidateAsync(Resource resource, CancellationToken cancellationToken);
    Task<CapabilityStatement> GetCapabilityStatementAsync(CancellationToken cancellationToken);
    Task<Resource> ExecuteOperationAsync(string operationName, Parameters parameters, CancellationToken cancellationToken);
    Task<Parameters> ExportAsync(Parameters parameters, CancellationToken cancellationToken);
    Task<Parameters> ImportAsync(Parameters parameters, CancellationToken cancellationToken);
}
