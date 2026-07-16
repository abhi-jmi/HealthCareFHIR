using FhirPlatform.Application.Contracts;
using FhirPlatform.FhirClient;
using Hl7.Fhir.Model;

namespace FhirPlatform.Application;

public interface IPatientService
{
    Task<IReadOnlyList<PatientSummaryDto>> SearchAsync(PatientSearchRequest request, CancellationToken cancellationToken);
    Task<Patient?> GetAsync(string id, CancellationToken cancellationToken);
    Task<Patient> CreateAsync(Patient patient, CancellationToken cancellationToken);
    Task<Patient> UpdateAsync(string id, Patient patient, CancellationToken cancellationToken);
}

public sealed class PatientService(IFhirResourceClient fhirClient) : IPatientService
{
    public async Task<IReadOnlyList<PatientSummaryDto>> SearchAsync(PatientSearchRequest request, CancellationToken cancellationToken)
    {
        var parameters = new Dictionary<string, string?> { ["_count"] = request.Count.ToString(), ["name"] = request.Name, ["identifier"] = request.Identifier, ["email"] = request.Email, ["phone"] = request.Phone, ["gender"] = request.Gender, ["active"] = request.Active?.ToString().ToLowerInvariant() };
        var bundle = await fhirClient.SearchAsync<Patient>(parameters, cancellationToken);
        return bundle.Entry.Select(e => e.Resource).OfType<Patient>().Select(ToSummary).ToArray();
    }

    public Task<Patient?> GetAsync(string id, CancellationToken cancellationToken) => fhirClient.ReadAsync<Patient>(id, cancellationToken);
    public Task<Patient> CreateAsync(Patient patient, CancellationToken cancellationToken) => fhirClient.CreateAsync(patient, cancellationToken);
    public Task<Patient> UpdateAsync(string id, Patient patient, CancellationToken cancellationToken) => fhirClient.UpdateAsync(id, patient, cancellationToken);

    private static PatientSummaryDto ToSummary(Patient patient) => new(patient.Id, patient.Name.FirstOrDefault()?.ToString() ?? patient.Id, patient.Gender?.ToString(), patient.BirthDate, patient.Active);
}
