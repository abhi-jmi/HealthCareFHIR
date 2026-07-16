using FhirPlatform.Application;
using FhirPlatform.Application.Contracts;
using FhirPlatform.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FhirPlatform.Api.Controllers;

[ApiController]
[Route("api/resources/{resourceType}")]
public sealed class ResourcesController(IResourceManagementService resources) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public Task<ResourceSearchResultDto> Search(string resourceType, [FromQuery] Dictionary<string, string?> parameters, CancellationToken cancellationToken) => resources.SearchAsync(resourceType, parameters, cancellationToken);

    [HttpGet("{id}")]
    [Authorize]
    public Task<ResourceReadResultDto> Read(string resourceType, string id, CancellationToken cancellationToken) => resources.ReadAsync(resourceType, id, cancellationToken);

    [HttpPost]
    [Authorize(Policy = Permissions.PatientWrite)]
    public Task<ResourceReadResultDto> Create(string resourceType, ResourceUpsertRequest request, CancellationToken cancellationToken) => resources.CreateAsync(resourceType, request.PayloadJson, cancellationToken);

    [HttpPut("{id}")]
    [Authorize(Policy = Permissions.PatientWrite)]
    public Task<ResourceReadResultDto> Update(string resourceType, string id, ResourceUpsertRequest request, CancellationToken cancellationToken) => resources.UpdateAsync(resourceType, id, request.PayloadJson, cancellationToken);
}
