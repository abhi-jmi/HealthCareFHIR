using FhirPlatform.Application;
using FhirPlatform.Application.Contracts;
using FhirPlatform.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FhirPlatform.Api.Controllers;

[ApiController]
[Route("api/fhir/validation")]
public sealed class ValidationController(IResourceValidationService validation) : ControllerBase
{
    [HttpPost, Authorize(Policy = Permissions.ConformanceManage)]
    public Task<FhirValidationResponse> Validate(FhirValidationRequest request, CancellationToken cancellationToken) =>
        validation.ValidateAsync(request, cancellationToken);
}
