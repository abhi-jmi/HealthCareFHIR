using FhirPlatform.Application.Contracts;
using FhirPlatform.FhirClient;
using Hl7.Fhir.Model;

namespace FhirPlatform.Application;

public interface IAuditEventService
{
    Task<AuditEventResult> RecordAsync(AuditEventRequest request, CancellationToken cancellationToken);
}

public sealed class AuditEventService(IFhirResourceClient fhirClient) : IAuditEventService
{
    public async Task<AuditEventResult> RecordAsync(AuditEventRequest request, CancellationToken cancellationToken)
    {
        var auditEvent = new AuditEvent
        {
            Type = new Coding("http://terminology.hl7.org/CodeSystem/audit-event-type", request.EventType, request.EventType),
            Recorded = new Instant(DateTimeOffset.UtcNow),
            Agent = { new AuditEvent.AgentComponent { Requestor = false, Name = "FHIR Platform" } }
        };
        if (!string.IsNullOrWhiteSpace(request.EntityReference)) auditEvent.Entity.Add(new AuditEvent.EntityComponent { What = new ResourceReference(request.EntityReference), Description = request.Description });
        var created = await fhirClient.CreateAsync(auditEvent, cancellationToken);
        return new AuditEventResult(created.Id, request.EventType, request.Outcome);
    }
}
