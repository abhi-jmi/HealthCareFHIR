using FhirPlatform.Application.Contracts;

namespace FhirPlatform.Application;

public interface IFhirBundleIngestionService
{
    Task<FhirBundleIngestionResponse> IngestAsync(FhirBundleIngestionRequest request, string initiatedBy, CancellationToken cancellationToken);
}

public interface IClinicalDocumentExtractionService
{
    Task<ClinicalDocumentExtractionResult> ExtractAsync(ClinicalDocumentExtractionRequest request, CancellationToken cancellationToken);
}
