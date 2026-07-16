namespace FhirPlatform.Application.Contracts;

public sealed record FhirResourceCapabilityDto(
    string ResourceType,
    IReadOnlyList<string> Interactions,
    IReadOnlyList<string> SearchParameters,
    IReadOnlyList<string> Operations);

public sealed record ConformanceDashboardDetailDto(
    bool Available,
    string? FhirVersion,
    int SupportedResourceCount,
    int SupportedSearchParameterCount,
    int SupportedOperationCount,
    DateTimeOffset RefreshedAt,
    IReadOnlyList<FhirResourceCapabilityDto> Resources,
    IReadOnlyList<string> MissingExpectedResources);
