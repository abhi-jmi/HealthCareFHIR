namespace FhirPlatform.Application.Contracts;

/// <summary>Request payload for validating a FHIR resource through the custom API.</summary>
public sealed record FhirValidationRequest(string Payload, string ExpectedResourceType, string Format);

/// <summary>FHIR validation result with normalized output and issue summaries.</summary>
public sealed record FhirValidationResponse(
    /// <summary>Indicates whether validation completed without errors.</summary>
    bool IsValid,
    /// <summary>The detected or expected FHIR resource type.</summary>
    string ResourceType,
    /// <summary>The normalized FHIR JSON representation.</summary>
    string NormalizedJson,
    /// <summary>The normalized FHIR XML representation.</summary>
    string NormalizedXml,
    /// <summary>The serialized OperationOutcome returned by validation.</summary>
    string OperationOutcomeJson,
    /// <summary>Error diagnostics returned by validation.</summary>
    IReadOnlyList<string> Errors,
    /// <summary>Warning diagnostics returned by validation.</summary>
    IReadOnlyList<string> Warnings,
    /// <summary>Custom extension URLs that are not registered in the platform extension registry.</summary>
    IReadOnlyList<string> UnknownExtensions);
