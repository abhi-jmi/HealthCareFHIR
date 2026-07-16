namespace FhirPlatform.Application.Contracts;

public sealed record ResourceSearchRequest(IReadOnlyDictionary<string, string?> Parameters, int Count = 20);
public sealed record ResourceSummaryDto(string ResourceType, string? Id, string? Display, string? Status, string? Date, string? SubjectReference);
public sealed record ResourceSearchResultDto(string ResourceType, int Total, IReadOnlyList<ResourceSummaryDto> Entries, string RawJson);
public sealed record ResourceReadResultDto(string ResourceType, string? Id, string RawJson);
public sealed record ResourceUpsertRequest(string PayloadJson);
