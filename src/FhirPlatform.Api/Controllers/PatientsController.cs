using FhirPlatform.Application;
using FhirPlatform.Application.Contracts;
using FhirPlatform.Domain.Shared;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FhirPlatform.Api.Controllers;

[ApiController]
[Route("api/patients")]
public sealed class PatientsController(IPatientService patients) : ControllerBase
{
    [HttpGet, Authorize(Policy = Permissions.PatientRead)]
    public Task<IReadOnlyList<PatientSummaryDto>> Search([FromQuery] PatientSearchRequest request, CancellationToken cancellationToken) => patients.SearchAsync(request, cancellationToken);

    [HttpGet("{id}"), Authorize(Policy = Permissions.PatientRead)]
    public async Task<ActionResult<Patient>> Get(string id, CancellationToken cancellationToken) => await patients.GetAsync(id, cancellationToken) is { } patient ? Ok(patient) : NotFound();

    [HttpPost, Authorize(Policy = Permissions.PatientWrite)]
    public async Task<ActionResult<Patient>> Create(Patient patient, CancellationToken cancellationToken) { var created = await patients.CreateAsync(patient, cancellationToken); return CreatedAtAction(nameof(Get), new { id = created.Id }, created); }

    [HttpPut("{id}"), Authorize(Policy = Permissions.PatientWrite)]
    public Task<Patient> Update(string id, Patient patient, CancellationToken cancellationToken) => patients.UpdateAsync(id, patient, cancellationToken);
}
