using FhirPlatform.Application;
using FhirPlatform.Application.Contracts;
using FhirPlatform.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FhirPlatform.Api.Controllers;

[ApiController]
[Route("api/resources/{resourceType}")]
public sealed class ResourcesController(IResourceManagementService resources, IAuthorizationService authorization) : ControllerBase
{
    private static readonly IReadOnlyDictionary<string, string> WritePolicies = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["Account"] = Permissions.FinancialManage,
        ["ActivityDefinition"] = Permissions.ClinicalReasoningExecute,
        ["AllergyIntolerance"] = Permissions.ClinicalWrite,
        ["Appointment"] = Permissions.WorkflowManage,
        ["CarePlan"] = Permissions.ClinicalWrite,
        ["CareTeam"] = Permissions.AdministrationManage,
        ["ChargeItem"] = Permissions.FinancialManage,
        ["Claim"] = Permissions.FinancialManage,
        ["ClaimResponse"] = Permissions.FinancialManage,
        ["ClinicalImpression"] = Permissions.ClinicalWrite,
        ["Communication"] = Permissions.WorkflowManage,
        ["CommunicationRequest"] = Permissions.WorkflowManage,
        ["Composition"] = Permissions.ClinicalWrite,
        ["Condition"] = Permissions.ClinicalWrite,
        ["Consent"] = Permissions.PatientWrite,
        ["Coverage"] = Permissions.FinancialManage,
        ["CoverageEligibilityRequest"] = Permissions.FinancialManage,
        ["CoverageEligibilityResponse"] = Permissions.FinancialManage,
        ["DetectedIssue"] = Permissions.ClinicalWrite,
        ["Device"] = Permissions.AdministrationManage,
        ["DiagnosticReport"] = Permissions.DiagnosticReportWrite,
        ["DocumentReference"] = Permissions.ClinicalWrite,
        ["Endpoint"] = Permissions.AdministrationManage,
        ["EpisodeOfCare"] = Permissions.WorkflowManage,
        ["Evidence"] = Permissions.ClinicalReasoningExecute,
        ["EvidenceVariable"] = Permissions.ClinicalReasoningExecute,
        ["ExplanationOfBenefit"] = Permissions.FinancialManage,
        ["FamilyMemberHistory"] = Permissions.ClinicalWrite,
        ["Flag"] = Permissions.ClinicalWrite,
        ["Goal"] = Permissions.ClinicalWrite,
        ["GuidanceResponse"] = Permissions.ClinicalReasoningExecute,
        ["HealthcareService"] = Permissions.AdministrationManage,
        ["ImagingStudy"] = Permissions.DiagnosticReportWrite,
        ["Immunization"] = Permissions.MedicationWrite,
        ["Invoice"] = Permissions.FinancialManage,
        ["Library"] = Permissions.ClinicalReasoningExecute,
        ["Location"] = Permissions.AdministrationManage,
        ["Measure"] = Permissions.ClinicalReasoningExecute,
        ["MeasureReport"] = Permissions.ClinicalReasoningExecute,
        ["Medication"] = Permissions.MedicationWrite,
        ["MedicationAdministration"] = Permissions.MedicationWrite,
        ["MedicationDispense"] = Permissions.MedicationWrite,
        ["MedicationRequest"] = Permissions.MedicationWrite,
        ["MedicationStatement"] = Permissions.MedicationWrite,
        ["Observation"] = Permissions.ObservationWrite,
        ["Organization"] = Permissions.AdministrationManage,
        ["Patient"] = Permissions.PatientWrite,
        ["PaymentNotice"] = Permissions.FinancialManage,
        ["PaymentReconciliation"] = Permissions.FinancialManage,
        ["PlanDefinition"] = Permissions.ClinicalReasoningExecute,
        ["Practitioner"] = Permissions.AdministrationManage,
        ["PractitionerRole"] = Permissions.AdministrationManage,
        ["Procedure"] = Permissions.ClinicalWrite,
        ["Questionnaire"] = Permissions.ClinicalWrite,
        ["QuestionnaireResponse"] = Permissions.ClinicalWrite,
        ["RelatedPerson"] = Permissions.PatientWrite,
        ["ResearchDefinition"] = Permissions.ClinicalReasoningExecute,
        ["ResearchElementDefinition"] = Permissions.ClinicalReasoningExecute,
        ["RiskAssessment"] = Permissions.ClinicalWrite,
        ["ServiceRequest"] = Permissions.WorkflowManage,
        ["Slot"] = Permissions.WorkflowManage,
        ["Specimen"] = Permissions.DiagnosticReportWrite,
        ["Task"] = Permissions.WorkflowManage
    };

    [HttpGet]
    [Authorize]
    public Task<ResourceSearchResultDto> Search(string resourceType, [FromQuery] Dictionary<string, string?> parameters, CancellationToken cancellationToken) => resources.SearchAsync(resourceType, parameters, cancellationToken);

    [HttpGet("{id}")]
    [Authorize]
    public Task<ResourceReadResultDto> Read(string resourceType, string id, CancellationToken cancellationToken) => resources.ReadAsync(resourceType, id, cancellationToken);

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ResourceReadResultDto>> Create(string resourceType, ResourceUpsertRequest request, CancellationToken cancellationToken)
    {
        var authorizationResult = await AuthorizeWriteAsync(resourceType);
        if (!authorizationResult.Succeeded) return Forbid();
        return await resources.CreateAsync(resourceType, request.PayloadJson, cancellationToken);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<ResourceReadResultDto>> Update(string resourceType, string id, ResourceUpsertRequest request, CancellationToken cancellationToken)
    {
        var authorizationResult = await AuthorizeWriteAsync(resourceType);
        if (!authorizationResult.Succeeded) return Forbid();
        return await resources.UpdateAsync(resourceType, id, request.PayloadJson, cancellationToken);
    }

    private Task<AuthorizationResult> AuthorizeWriteAsync(string resourceType)
    {
        var policy = WritePolicies.TryGetValue(resourceType, out var mappedPolicy) ? mappedPolicy : Permissions.ConformanceManage;
        return authorization.AuthorizeAsync(User, policy);
    }
}
