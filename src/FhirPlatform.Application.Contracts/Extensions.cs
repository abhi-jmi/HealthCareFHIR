namespace FhirPlatform.Application.Contracts;

public sealed record ExtensionRegistryEntryDto(
    Guid Id,
    string CanonicalUrl,
    string Name,
    string? Description,
    IReadOnlyList<string> ApplicableResourceTypes,
    string ValueType,
    bool Active);

public sealed record UpsertExtensionRegistryEntryRequest(
    string CanonicalUrl,
    string Name,
    string? Description,
    IReadOnlyList<string> ApplicableResourceTypes,
    string ValueType,
    bool Active = true);
