namespace FhirPlatform.Domain.Shared;

/// <summary>Maps platform roles to least-privilege permission grants used by API policies and seed identity data.</summary>
public static class RolePermissionMap
{
    private static readonly IReadOnlyDictionary<string, string[]> Grants = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
    {
        [PlatformRoles.SystemAdministrator] = Permissions.All.ToArray(),
        [PlatformRoles.FhirAdministrator] = [Permissions.PatientRead, Permissions.PatientWrite, Permissions.ConformanceManage, Permissions.TerminologyManage, Permissions.AuditRead],
        [PlatformRoles.Clinician] = [Permissions.PatientRead, Permissions.PatientWrite, Permissions.ObservationRead, Permissions.ObservationWrite, Permissions.DiagnosticReportRead, Permissions.MedicationRead, Permissions.WorkflowManage],
        [PlatformRoles.DiagnosticReviewer] = [Permissions.PatientRead, Permissions.ObservationRead, Permissions.DiagnosticReportRead, Permissions.DiagnosticReportWrite],
        [PlatformRoles.Patient] = [Permissions.PatientRead, Permissions.ObservationRead, Permissions.DiagnosticReportRead, Permissions.MedicationRead],
        [PlatformRoles.Auditor] = [Permissions.AuditRead],
        [PlatformRoles.IntegrationClient] = [Permissions.PatientRead, Permissions.PatientWrite, Permissions.ObservationRead, Permissions.ObservationWrite, Permissions.DiagnosticReportRead, Permissions.DiagnosticReportWrite]
    };

    public static bool RoleHasPermission(string role, string permission) =>
        Grants.TryGetValue(role, out var permissions) && permissions.Contains(permission, StringComparer.OrdinalIgnoreCase);

    public static IReadOnlyCollection<string> GetPermissions(string role) =>
        Grants.TryGetValue(role, out var permissions) ? permissions : Array.Empty<string>();
}
