using FhirPlatform.Application;
using FhirPlatform.Application.Contracts;
using FhirPlatform.FhirClient;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace FhirPlatform.Infrastructure;

public sealed class FhirBundleIngestionService(AppDbContext db, IFhirResourceClient fhirClient) : IFhirBundleIngestionService
{
    private readonly FhirJsonDeserializer _jsonDeserializer = new();
    public async Task<FhirBundleIngestionResponse> IngestAsync(FhirBundleIngestionRequest request, string initiatedBy, CancellationToken cancellationToken)
    {
        var existing = await db.IngestionJobs.SingleOrDefaultAsync(x => x.IdempotencyKey == request.IdempotencyKey, cancellationToken);
        if (existing is not null) return new FhirBundleIngestionResponse(existing.Id, existing.Status, existing.OperationOutcomeJson ?? string.Empty);
        var job = new IngestionJob { Id = Guid.NewGuid(), IdempotencyKey = request.IdempotencyKey, Status = "Running" };
        db.IngestionJobs.Add(job); await db.SaveChangesAsync(cancellationToken);
        var outcome = new OperationOutcome();
        try
        {
            var bundle = _jsonDeserializer.Deserialize<Bundle>(request.BundleJson);
            if (bundle.Type is not (Bundle.BundleType.Transaction or Bundle.BundleType.Batch)) AddIssue(outcome, OperationOutcome.IssueSeverity.Error, "Bundle must be transaction or batch.");
            if (!bundle.Entry.Any(e => e.Resource is Patient)) AddIssue(outcome, OperationOutcome.IssueSeverity.Warning, "Bundle does not include a Patient resource; references must resolve on the FHIR server.");
            foreach (var observation in bundle.Entry.Select(e => e.Resource).OfType<Observation>())
            {
                if (observation.Code is null) AddIssue(outcome, OperationOutcome.IssueSeverity.Error, $"Observation/{observation.Id} is missing code.");
                if (observation.Value is Quantity quantity && string.IsNullOrWhiteSpace(quantity.System)) AddIssue(outcome, OperationOutcome.IssueSeverity.Warning, $"Observation/{observation.Id} quantity should include a UCUM system.");
            }
            if (outcome.Issue.Any(i => i.Severity is OperationOutcome.IssueSeverity.Error or OperationOutcome.IssueSeverity.Fatal)) throw new InvalidOperationException("Bundle validation failed.");
            var provenance = new Provenance { Recorded = DateTimeOffset.UtcNow, Activity = new CodeableConcept("http://terminology.hl7.org/CodeSystem/v3-DataOperation", "CREATE", "create") };
            bundle.Entry.Add(new Bundle.EntryComponent { Resource = provenance, Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = "Provenance" } });
            await fhirClient.TransactionAsync(bundle, cancellationToken);
            AddIssue(outcome, OperationOutcome.IssueSeverity.Information, "Bundle ingested through FHIR REST API.");
            job.Status = "Completed";
        }
        catch (Exception ex) { AddIssue(outcome, OperationOutcome.IssueSeverity.Error, ex.Message); job.Status = "Failed"; }
        job.CompletedAt = DateTimeOffset.UtcNow; job.OperationOutcomeJson = outcome.ToJson(pretty: true); await db.SaveChangesAsync(cancellationToken);
        return new FhirBundleIngestionResponse(job.Id, job.Status, job.OperationOutcomeJson);
    }
    private static void AddIssue(OperationOutcome outcome, OperationOutcome.IssueSeverity severity, string diagnostics) => outcome.Issue.Add(new OperationOutcome.IssueComponent { Severity = severity, Code = OperationOutcome.IssueType.Processing, Diagnostics = diagnostics });
}

public sealed class DeterministicClinicalDocumentExtractionService : IClinicalDocumentExtractionService
{
    /// <summary>Returns a deterministic placeholder extraction result without fabricating clinical data.</summary>
    public Task<ClinicalDocumentExtractionResult> ExtractAsync(ClinicalDocumentExtractionRequest request, CancellationToken cancellationToken) => Task.FromResult(new ClinicalDocumentExtractionResult(null, null, [], [], [], [], [], [], 0, null, null, ["Document extraction is an integration seam only; no fabricated clinical data is generated."]));
}
