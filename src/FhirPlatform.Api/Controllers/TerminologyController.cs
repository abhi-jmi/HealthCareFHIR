using FhirPlatform.Application;
using FhirPlatform.Application.Contracts;
using FhirPlatform.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FhirPlatform.Api.Controllers;

[ApiController]
[Route("api/terminology")]
[Authorize(Policy = Permissions.TerminologyManage)]
public sealed class TerminologyController(ITerminologyService terminology) : ControllerBase
{
    [HttpGet("code-systems")]
    public Task<IReadOnlyList<TerminologyResourceSummaryDto>> CodeSystems([FromQuery] string? filter, [FromQuery] int count, CancellationToken cancellationToken) =>
        terminology.SearchCodeSystemsAsync(new TerminologySearchRequest(filter, count == 0 ? 20 : count), cancellationToken);

    [HttpGet("value-sets")]
    public Task<IReadOnlyList<TerminologyResourceSummaryDto>> ValueSets([FromQuery] string? filter, [FromQuery] int count, CancellationToken cancellationToken) =>
        terminology.SearchValueSetsAsync(new TerminologySearchRequest(filter, count == 0 ? 20 : count), cancellationToken);

    [HttpGet("concept-maps")]
    public Task<IReadOnlyList<TerminologyResourceSummaryDto>> ConceptMaps([FromQuery] string? filter, [FromQuery] int count, CancellationToken cancellationToken) =>
        terminology.SearchConceptMapsAsync(new TerminologySearchRequest(filter, count == 0 ? 20 : count), cancellationToken);

    [HttpPost("value-set/$expand")]
    public Task<IReadOnlyList<TerminologyCodingDto>> Expand(ValueSetExpandRequest request, CancellationToken cancellationToken) => terminology.ExpandValueSetAsync(request, cancellationToken);

    [HttpPost("value-set/$validate-code")]
    public Task<CodeValidationResponse> ValidateCode(CodeValidationRequest request, CancellationToken cancellationToken) => terminology.ValidateCodeAsync(request, cancellationToken);

    [HttpPost("concept-map/$translate")]
    public Task<ConceptTranslateResponse> Translate(ConceptTranslateRequest request, CancellationToken cancellationToken) => terminology.TranslateAsync(request, cancellationToken);
}
