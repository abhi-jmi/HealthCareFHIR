using FhirPlatform.Application;
using FhirPlatform.Application.Contracts;
using FhirPlatform.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FhirPlatform.Api.Controllers;

[ApiController]
[Route("api/administration/extensions")]
[Authorize(Policy = Permissions.ConformanceManage)]
public sealed class ExtensionRegistryController(IExtensionRegistryService registry) : ControllerBase
{
    [HttpGet]
    public Task<IReadOnlyList<ExtensionRegistryEntryDto>> List(CancellationToken cancellationToken) => registry.ListAsync(cancellationToken);

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExtensionRegistryEntryDto>> Get(Guid id, CancellationToken cancellationToken) =>
        await registry.GetAsync(id, cancellationToken) is { } entry ? Ok(entry) : NotFound();

    [HttpPost]
    public async Task<ActionResult<ExtensionRegistryEntryDto>> Create(UpsertExtensionRegistryEntryRequest request, CancellationToken cancellationToken)
    {
        var created = await registry.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ExtensionRegistryEntryDto>> Update(Guid id, UpsertExtensionRegistryEntryRequest request, CancellationToken cancellationToken) =>
        await registry.UpdateAsync(id, request, cancellationToken) is { } updated ? Ok(updated) : NotFound();

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken) =>
        await registry.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}
