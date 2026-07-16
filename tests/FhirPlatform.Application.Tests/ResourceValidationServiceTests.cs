using System.Net;
using FhirPlatform.Application;
using FhirPlatform.Application.Contracts;
using FhirPlatform.FhirClient;
using FluentAssertions;
using Hl7.Fhir.Model;

namespace FhirPlatform.Application.Tests;

public sealed class ResourceValidationServiceTests
{
    [Fact]
    public async Task ValidateAsync_accepts_json_and_returns_normalized_json_and_xml()
    {
        var service = new ResourceValidationService(new ValidatingFhirClient(), new StaticExtensionRegistry(Array.Empty<string>()));
        var response = await service.ValidateAsync(new FhirValidationRequest("{\"resourceType\":\"Patient\",\"id\":\"p1\"}", "Patient", "json"), CancellationToken.None);
        response.IsValid.Should().BeTrue();
        response.NormalizedJson.Should().Contain("\"resourceType\": \"Patient\"");
        response.NormalizedXml.Should().Contain("Patient");
    }

    [Fact]
    public async Task ValidateAsync_accepts_xml()
    {
        var service = new ResourceValidationService(new ValidatingFhirClient(), new StaticExtensionRegistry(Array.Empty<string>()));
        var response = await service.ValidateAsync(new FhirValidationRequest("<Patient xmlns=\"http://hl7.org/fhir\"><id value=\"p1\" /></Patient>", "Patient", "xml"), CancellationToken.None);
        response.IsValid.Should().BeTrue();
        response.ResourceType.Should().Be("Patient");
    }

    [Fact]
    public async Task ValidateAsync_maps_operation_outcome_errors()
    {
        var service = new ResourceValidationService(new InvalidFhirClient("Invalid birthDate"), new StaticExtensionRegistry(Array.Empty<string>()));
        var response = await service.ValidateAsync(new FhirValidationRequest("{\"resourceType\":\"Patient\"}", "Patient", "json"), CancellationToken.None);
        response.IsValid.Should().BeFalse();
        response.Errors.Should().Contain("Invalid birthDate");
        response.OperationOutcomeJson.Should().Contain("Invalid birthDate");
    }

    [Fact]
    public async Task ValidateAsync_rejects_unknown_custom_extensions()
    {
        var service = new ResourceValidationService(new ValidatingFhirClient(), new StaticExtensionRegistry(Array.Empty<string>()));
        var payload = "{\"resourceType\":\"Patient\",\"extension\":[{\"url\":\"https://example.org/fhir/StructureDefinition/custom\",\"valueString\":\"x\"}]}";
        var response = await service.ValidateAsync(new FhirValidationRequest(payload, "Patient", "json"), CancellationToken.None);
        response.IsValid.Should().BeFalse();
        response.UnknownExtensions.Should().Contain("https://example.org/fhir/StructureDefinition/custom");
    }

    [Fact]
    public async Task ValidateAsync_allows_registered_custom_extensions()
    {
        var service = new ResourceValidationService(new ValidatingFhirClient(), new StaticExtensionRegistry(["https://example.org/fhir/StructureDefinition/custom"]));
        var payload = "{\"resourceType\":\"Patient\",\"extension\":[{\"url\":\"https://example.org/fhir/StructureDefinition/custom\",\"valueString\":\"x\"}]}";
        var response = await service.ValidateAsync(new FhirValidationRequest(payload, "Patient", "json"), CancellationToken.None);
        response.IsValid.Should().BeTrue();
        response.UnknownExtensions.Should().BeEmpty();
    }

    [Fact]
    public async Task ValidateAsync_validates_references_quantities_and_bundles_by_delegating_to_fhir_validate()
    {
        var client = new CapturingFhirClient();
        var service = new ResourceValidationService(client, new StaticExtensionRegistry(Array.Empty<string>()));
        var bundle = "{\"resourceType\":\"Bundle\",\"type\":\"transaction\",\"entry\":[{\"resource\":{\"resourceType\":\"Observation\",\"status\":\"final\",\"code\":{\"text\":\"Body weight\"},\"subject\":{\"reference\":\"Patient/p1\"},\"valueQuantity\":{\"value\":70,\"system\":\"http://unitsofmeasure.org\",\"code\":\"kg\"}},\"request\":{\"method\":\"POST\",\"url\":\"Observation\"}}]}";
        var response = await service.ValidateAsync(new FhirValidationRequest(bundle, "Bundle", "json"), CancellationToken.None);
        response.IsValid.Should().BeTrue();
        client.LastValidatedResource.Should().BeOfType<Bundle>();
    }

    private sealed class StaticExtensionRegistry(IEnumerable<string> urls) : IExtensionRegistryReader
    {
        public Task<IReadOnlySet<string>> GetActiveCanonicalUrlsAsync(string resourceType, CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlySet<string>>(urls.ToHashSet(StringComparer.OrdinalIgnoreCase));
    }

    private class ValidatingFhirClient : IFhirResourceClient
    {
        public virtual Task<OperationOutcome> ValidateAsync(Resource resource, CancellationToken cancellationToken) => Task.FromResult(new OperationOutcome());
        public Task<T?> ReadAsync<T>(string id, CancellationToken cancellationToken) where T : Resource, new() => Task.FromResult<T?>(null);
        public Task<T?> VersionReadAsync<T>(string id, string versionId, CancellationToken cancellationToken) where T : Resource, new() => Task.FromResult<T?>(null);
        public Task<Bundle> SearchAsync<T>(IReadOnlyDictionary<string, string?> parameters, CancellationToken cancellationToken) where T : Resource, new() => Task.FromResult(new Bundle());
        public Task<T> CreateAsync<T>(T resource, CancellationToken cancellationToken) where T : Resource, new() => Task.FromResult(resource);
        public Task<T> UpdateAsync<T>(string id, T resource, CancellationToken cancellationToken) where T : Resource, new() => Task.FromResult(resource);
        public Task<Resource> PatchAsync(string resourceType, string id, Parameters patch, CancellationToken cancellationToken) => Task.FromResult<Resource>(patch);
        public Task DeleteAsync<T>(string id, CancellationToken cancellationToken) where T : Resource, new() => Task.CompletedTask;
        public Task<Bundle> HistoryAsync<T>(string id, CancellationToken cancellationToken) where T : Resource, new() => Task.FromResult(new Bundle());
        public Task<Bundle> TransactionAsync(Bundle bundle, CancellationToken cancellationToken) => Task.FromResult(bundle);
        public Task<Bundle> BatchAsync(Bundle bundle, CancellationToken cancellationToken) => Task.FromResult(bundle);
        public Task<CapabilityStatement> GetCapabilityStatementAsync(CancellationToken cancellationToken) => Task.FromResult(new CapabilityStatement());
        public Task<Resource> ExecuteOperationAsync(string operationName, Parameters parameters, CancellationToken cancellationToken) => Task.FromResult<Resource>(parameters);
        public Task<Parameters> ExportAsync(Parameters parameters, CancellationToken cancellationToken) => Task.FromResult(parameters);
        public Task<Parameters> ImportAsync(Parameters parameters, CancellationToken cancellationToken) => Task.FromResult(parameters);
    }

    private sealed class InvalidFhirClient(string diagnostic) : ValidatingFhirClient
    {
        public override Task<OperationOutcome> ValidateAsync(Resource resource, CancellationToken cancellationToken) => Task.FromResult(new OperationOutcome
        {
            Issue = { new OperationOutcome.IssueComponent { Severity = OperationOutcome.IssueSeverity.Error, Code = OperationOutcome.IssueType.Invalid, Diagnostics = diagnostic } }
        });
    }

    private sealed class CapturingFhirClient : ValidatingFhirClient
    {
        public Resource? LastValidatedResource { get; private set; }
        public override Task<OperationOutcome> ValidateAsync(Resource resource, CancellationToken cancellationToken)
        {
            LastValidatedResource = resource;
            return base.ValidateAsync(resource, cancellationToken);
        }
    }
}
