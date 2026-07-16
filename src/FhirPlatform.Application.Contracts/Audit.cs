namespace FhirPlatform.Application.Contracts;

public sealed record AuditEventRequest(string EventType, string Action, string Outcome, string? EntityReference, string? Description);
public sealed record AuditEventResult(string? AuditEventId, string EventType, string Outcome);
