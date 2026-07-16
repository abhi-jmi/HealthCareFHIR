using FhirPlatform.Application;
using FhirPlatform.Application.Contracts;
using FhirPlatform.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FhirPlatform.Api.Controllers;

[ApiController]
[Route("api/clinical-reasoning")]
[Authorize(Policy = Permissions.ClinicalReasoningExecute)]
public sealed class ClinicalReasoningController(IClinicalReasoningService service) : ControllerBase
{
    [HttpGet("rules")]
    public Task<IReadOnlyList<RuleConfigurationDto>> Rules(CancellationToken cancellationToken) => service.ListRulesAsync(cancellationToken);

    [HttpPut("rules")]
    public Task<RuleConfigurationDto> Upsert(UpsertRuleConfigurationRequest request, CancellationToken cancellationToken) => service.UpsertRuleAsync(request, cancellationToken);

    [HttpPost("execute")]
    public Task<RuleExecutionResultDto> Execute(ExecuteRuleRequest request, CancellationToken cancellationToken) => service.ExecuteAsync(request, User.Identity?.Name ?? "system", cancellationToken);
}
