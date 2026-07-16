namespace FhirPlatform.Application.Contracts;

public sealed record TerminologySearchRequest(string? Filter, int Count = 20);
public sealed record TerminologyResourceSummaryDto(string ResourceType, string? Id, string? Url, string? Version, string? Name, string? Title, string? Status);
public sealed record ValueSetExpandRequest(string ValueSetUrl, string? Filter = null, int Count = 50);
public sealed record CodeValidationRequest(string ValueSetUrl, string System, string Code, string? Display = null);
public sealed record CodeValidationResponse(bool Result, string? Message, string? Display);
public sealed record ConceptTranslateRequest(string ConceptMapUrl, string System, string Code, string? TargetSystem = null);
public sealed record ConceptTranslateResponse(bool Result, string? Message, IReadOnlyList<TerminologyCodingDto> Matches);
public sealed record TerminologyCodingDto(string? System, string? Code, string? Display);
