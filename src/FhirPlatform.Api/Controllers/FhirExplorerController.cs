using FhirPlatform.Application;
using FhirPlatform.Application.Contracts;
using FhirPlatform.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FhirPlatform.Api.Controllers;

[ApiController]
[Route("api/fhir-explorer")]
[Authorize(Policy = Permissions.ConformanceManage)]
public sealed class FhirExplorerController(IFhirExplorerService explorer) : ControllerBase
{
    [HttpPost("execute")]
    public Task<FhirExplorerResponse> Execute(FhirExplorerRequest request, CancellationToken cancellationToken) => explorer.ExecuteAsync(request, cancellationToken);
}
