using FhirPlatform.Application;
using FhirPlatform.Application.Contracts;
using FhirPlatform.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FhirPlatform.Api.Controllers;

[ApiController]
[Route("api/audit")]
[Authorize(Policy = Permissions.AuditRead)]
public sealed class AuditController(IAuditEventService auditEvents) : ControllerBase
{
    [HttpPost("events")]
    public Task<AuditEventResult> Record(AuditEventRequest request, CancellationToken cancellationToken) => auditEvents.RecordAsync(request, cancellationToken);
}
