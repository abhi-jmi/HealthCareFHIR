using FhirPlatform.Application;
using FhirPlatform.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FhirPlatform.Api.Controllers;

[ApiController]
[Route("api/fhir-levels")]
public sealed class FhirLevelsController(IFhirLevelCatalogService catalog) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public FhirLevelCatalogDto Get() => catalog.GetCatalog();
}
