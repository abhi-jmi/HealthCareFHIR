using System.Net.Http.Headers;
using System.Text;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.Extensions.Logging;

namespace FhirPlatform.FhirClient;

public sealed class MicrosoftFhirResourceClient(HttpClient httpClient, ILogger<MicrosoftFhirResourceClient> logger) : IFhirResourceClient
{
    private readonly FhirJsonParser _parser = new();
    private readonly FhirJsonSerializer _serializer = new(new SerializerSettings { Pretty = false });

    public async Task<T?> ReadAsync<T>(string id, CancellationToken cancellationToken) where T : Resource, new() =>
        await SendAsync<T>(HttpMethod.Get, $"{new T().TypeName}/{Uri.EscapeDataString(id)}", null, cancellationToken);

    public async Task<Resource?> ReadAsync(string resourceType, string id, CancellationToken cancellationToken) =>
        await SendAsync<Resource>(HttpMethod.Get, $"{Uri.EscapeDataString(resourceType)}/{Uri.EscapeDataString(id)}", null, cancellationToken);

    public async Task<T?> VersionReadAsync<T>(string id, string versionId, CancellationToken cancellationToken) where T : Resource, new() =>
        await SendAsync<T>(HttpMethod.Get, $"{new T().TypeName}/{Uri.EscapeDataString(id)}/_history/{Uri.EscapeDataString(versionId)}", null, cancellationToken);

    public Task<Bundle> SearchAsync<T>(IReadOnlyDictionary<string, string?> parameters, CancellationToken cancellationToken) where T : Resource, new() =>
        SearchAsync(new T().TypeName, parameters, cancellationToken);

    public async Task<Bundle> SearchAsync(string resourceType, IReadOnlyDictionary<string, string?> parameters, CancellationToken cancellationToken)
    {
        var query = string.Join('&', parameters.Where(p => !string.IsNullOrWhiteSpace(p.Value)).Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value!)}"));
        return await SendAsync<Bundle>(HttpMethod.Get, $"{Uri.EscapeDataString(resourceType)}?{query}", null, cancellationToken) ?? new Bundle();
    }

    public async Task<T> CreateAsync<T>(T resource, CancellationToken cancellationToken) where T : Resource, new() =>
        (await SendAsync<T>(HttpMethod.Post, resource.TypeName, resource, cancellationToken))!;

    public async Task<T> UpdateAsync<T>(string id, T resource, CancellationToken cancellationToken) where T : Resource, new() =>
        (await SendAsync<T>(HttpMethod.Put, $"{resource.TypeName}/{Uri.EscapeDataString(id)}", resource, cancellationToken))!;

    public async Task<Resource> PatchAsync(string resourceType, string id, Parameters patch, CancellationToken cancellationToken) =>
        (await SendAsync<Resource>(HttpMethod.Patch, $"{resourceType}/{Uri.EscapeDataString(id)}", patch, cancellationToken))!;

    public async Task DeleteAsync<T>(string id, CancellationToken cancellationToken) where T : Resource, new() =>
        await SendAsync<Resource>(HttpMethod.Delete, $"{new T().TypeName}/{Uri.EscapeDataString(id)}", null, cancellationToken);

    public async Task<Bundle> HistoryAsync<T>(string id, CancellationToken cancellationToken) where T : Resource, new() =>
        (await SendAsync<Bundle>(HttpMethod.Get, $"{new T().TypeName}/{Uri.EscapeDataString(id)}/_history", null, cancellationToken))!;

    public Task<Bundle> TransactionAsync(Bundle bundle, CancellationToken cancellationToken) => SendBundleAsync(bundle, "transaction", cancellationToken);
    public Task<Bundle> BatchAsync(Bundle bundle, CancellationToken cancellationToken) => SendBundleAsync(bundle, "batch", cancellationToken);
    public async Task<OperationOutcome> ValidateAsync(Resource resource, CancellationToken cancellationToken) =>
        (await SendAsync<OperationOutcome>(HttpMethod.Post, $"{resource.TypeName}/$validate", resource, cancellationToken))!;
    public async Task<CapabilityStatement> GetCapabilityStatementAsync(CancellationToken cancellationToken) =>
        (await SendAsync<CapabilityStatement>(HttpMethod.Get, "metadata", null, cancellationToken))!;
    public async Task<Resource> ExecuteOperationAsync(string operationName, Parameters parameters, CancellationToken cancellationToken)
    {
        var normalizedOperation = operationName.Contains('/', StringComparison.Ordinal)
            ? operationName.TrimStart('/')
            : '$' + operationName.TrimStart('$');
        return (await SendAsync<Resource>(HttpMethod.Post, normalizedOperation, parameters, cancellationToken))!;
    }
    public async Task<Parameters> ExportAsync(Parameters parameters, CancellationToken cancellationToken) =>
        (await SendAsync<Parameters>(HttpMethod.Post, "$export", parameters, cancellationToken))!;
    public async Task<Parameters> ImportAsync(Parameters parameters, CancellationToken cancellationToken) =>
        (await SendAsync<Parameters>(HttpMethod.Post, "$import", parameters, cancellationToken))!;

    private async Task<Bundle> SendBundleAsync(Bundle bundle, string type, CancellationToken cancellationToken)
    {
        bundle.Type = type == "batch" ? Bundle.BundleType.Batch : Bundle.BundleType.Transaction;
        return (await SendAsync<Bundle>(HttpMethod.Post, string.Empty, bundle, cancellationToken))!;
    }

    private async Task<T?> SendAsync<T>(HttpMethod method, string path, Resource? body, CancellationToken cancellationToken) where T : Resource
    {
        using var request = new HttpRequestMessage(method, path);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/fhir+json"));
        request.Headers.TryAddWithoutValidation("X-Correlation-ID", Guid.NewGuid().ToString("n"));
        if (body is not null) request.Content = new StringContent(_serializer.SerializeToString(body), Encoding.UTF8, "application/fhir+json");
        using var response = await httpClient.SendAsync(request, cancellationToken);
        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        logger.LogInformation("FHIR {Method} {Path} returned {StatusCode}", method, path, (int)response.StatusCode);
        if (!response.IsSuccessStatusCode) throw new FhirOperationException(response.StatusCode, TryParseOutcome(payload));
        return string.IsNullOrWhiteSpace(payload) ? null : _parser.Parse<T>(payload);
    }

    private OperationOutcome? TryParseOutcome(string payload)
    {
        try { return string.IsNullOrWhiteSpace(payload) ? null : _parser.Parse<OperationOutcome>(payload); }
        catch (Exception ex) { logger.LogWarning(ex, "Unable to parse OperationOutcome from failed FHIR response."); return null; }
    }
}

public sealed class FhirOperationException(System.Net.HttpStatusCode statusCode, OperationOutcome? outcome) : Exception($"FHIR server returned {(int)statusCode}")
{
    public System.Net.HttpStatusCode StatusCode { get; } = statusCode;
    public OperationOutcome? Outcome { get; } = outcome;
}
