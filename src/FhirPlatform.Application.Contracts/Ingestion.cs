namespace FhirPlatform.Application.Contracts;

public sealed record FhirBundleIngestionRequest(string BundleJson, string IdempotencyKey);
public sealed record FhirBundleIngestionResponse(Guid JobId, string Status, string OperationOutcomeJson);
public sealed record ClinicalDocumentExtractionRequest(string DocumentReference, string ContentType);
public sealed record ClinicalDocumentExtractionResult(
    string? PatientMetadata,
    string? DiagnosticReportMetadata,
    IReadOnlyList<string> Observations,
    IReadOnlyList<string> Codes,
    IReadOnlyList<string> Values,
    IReadOnlyList<string> Units,
    IReadOnlyList<string> ReferenceRanges,
    IReadOnlyList<string> Interpretations,
    decimal ConfidenceScore,
    int? SourcePage,
    string? SourceText,
    IReadOnlyList<string> ValidationWarnings);
