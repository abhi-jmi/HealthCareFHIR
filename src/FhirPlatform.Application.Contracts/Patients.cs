namespace FhirPlatform.Application.Contracts;

public sealed record PatientSummaryDto(string Id, string DisplayName, string? Gender, string? BirthDate, bool? Active);
public sealed record PatientSearchRequest(string? Name, string? Identifier, string? Email, string? Phone, string? Gender, bool? Active, int Count = 20);
public sealed record CapabilityDashboardDto(bool Available, string? FhirVersion, int ResourceCount, int SearchParameterCount, DateTimeOffset RefreshedAt);
