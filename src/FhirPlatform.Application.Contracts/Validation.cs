namespace FhirPlatform.Application.Contracts;

public sealed record FhirValidationRequest(string Payload, string ExpectedResourceType, string Format);

public sealed record FhirValidationResponse(
    bool IsValid,
    string ResourceType,
    string NormalizedJson,
    string NormalizedXml,
    string OperationOutcomeJson,
    IReadOnlyList<string> Errors,
    IReadOnlyList<string> Warnings);
