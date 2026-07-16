using FhirPlatform.Application.Contracts;

namespace FhirPlatform.Application;

public interface IExtensionRegistryReader
{
    Task<IReadOnlySet<string>> GetActiveCanonicalUrlsAsync(string resourceType, CancellationToken cancellationToken);
}

public interface IExtensionRegistryService : IExtensionRegistryReader
{
    Task<IReadOnlyList<ExtensionRegistryEntryDto>> ListAsync(CancellationToken cancellationToken);
    Task<ExtensionRegistryEntryDto?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<ExtensionRegistryEntryDto> CreateAsync(UpsertExtensionRegistryEntryRequest request, CancellationToken cancellationToken);
    Task<ExtensionRegistryEntryDto?> UpdateAsync(Guid id, UpsertExtensionRegistryEntryRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
