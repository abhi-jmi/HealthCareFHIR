namespace FhirPlatform.Domain.Shared;

/// <summary>Application permission constants enforced by API authorization policies.</summary>
public static class Permissions
{
    public const string PatientRead = "Patient.Read";
    public const string PatientWrite = "Patient.Write";
    public const string ConformanceManage = "Conformance.Manage";
    public const string AuditRead = "Audit.Read";
}

/// <summary>Supported development and production roles.</summary>
public static class PlatformRoles
{
    public const string SystemAdministrator = "SystemAdministrator";
    public const string FhirAdministrator = "FhirAdministrator";
    public const string Clinician = "Clinician";
    public const string Auditor = "Auditor";
}
