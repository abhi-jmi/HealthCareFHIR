using FhirPlatform.Application;
using FhirPlatform.Application.Contracts;
using FhirPlatform.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FhirPlatform.Api.Controllers;

[ApiController]
[Route("api/conformance")]
public sealed class ConformanceController(IConformanceService conformance) : ControllerBase
{
    [HttpGet("dashboard"), Authorize(Policy = Permissions.ConformanceManage)]
    public Task<CapabilityDashboardDto> Dashboard(CancellationToken cancellationToken) => conformance.GetDashboardAsync(cancellationToken);
}
