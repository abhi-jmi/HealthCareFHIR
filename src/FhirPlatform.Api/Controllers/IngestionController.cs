using FhirPlatform.Application;
using FhirPlatform.Application.Contracts;
using FhirPlatform.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FhirPlatform.Api.Controllers;

[ApiController]
[Route("api/ingestion")]
public sealed class IngestionController(IFhirBundleIngestionService ingestion, IClinicalDocumentExtractionService extraction) : ControllerBase
{
    [HttpPost("fhir-bundle")]
    [Authorize(Policy = Permissions.ObservationWrite)]
    public Task<FhirBundleIngestionResponse> Ingest(FhirBundleIngestionRequest request, CancellationToken cancellationToken) => ingestion.IngestAsync(request, User.Identity?.Name ?? "system", cancellationToken);

    [HttpPost("document-extraction/preview")]
    [Authorize(Policy = Permissions.DiagnosticReportWrite)]
    public Task<ClinicalDocumentExtractionResult> Extract(ClinicalDocumentExtractionRequest request, CancellationToken cancellationToken) => extraction.ExtractAsync(request, cancellationToken);
}
