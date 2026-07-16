namespace FhirPlatform.Domain.Shared;

/// <summary>Application permission constants enforced by API authorization policies.</summary>
public static class Permissions
{
    public const string PatientRead = "Patient.Read";
    public const string PatientWrite = "Patient.Write";
    public const string ObservationRead = "Observation.Read";
    public const string ObservationWrite = "Observation.Write";
    public const string DiagnosticReportRead = "DiagnosticReport.Read";
    public const string DiagnosticReportWrite = "DiagnosticReport.Write";
    public const string MedicationRead = "Medication.Read";
    public const string MedicationWrite = "Medication.Write";
    public const string WorkflowManage = "Workflow.Manage";
    public const string TerminologyManage = "Terminology.Manage";
    public const string ConformanceManage = "Conformance.Manage";
    public const string AuditRead = "Audit.Read";
    public const string ClinicalReasoningExecute = "ClinicalReasoning.Execute";

    public static readonly IReadOnlySet<string> All = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        PatientRead, PatientWrite, ObservationRead, ObservationWrite, DiagnosticReportRead, DiagnosticReportWrite,
        MedicationRead, MedicationWrite, WorkflowManage, TerminologyManage, ConformanceManage, AuditRead,
        ClinicalReasoningExecute
    };
}

/// <summary>Supported development and production roles.</summary>
public static class PlatformRoles
{
    public const string SystemAdministrator = "SystemAdministrator";
    public const string FhirAdministrator = "FhirAdministrator";
    public const string Clinician = "Clinician";
    public const string DiagnosticReviewer = "DiagnosticReviewer";
    public const string Patient = "Patient";
    public const string Auditor = "Auditor";
    public const string IntegrationClient = "IntegrationClient";
}
