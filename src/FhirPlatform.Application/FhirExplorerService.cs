using FhirPlatform.Application.Contracts;
using FhirPlatform.FhirClient;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;

namespace FhirPlatform.Application;

public interface IFhirExplorerService
{
    Task<FhirExplorerResponse> ExecuteAsync(FhirExplorerRequest request, CancellationToken cancellationToken);
}

public sealed class FhirExplorerService(IFhirResourceClient fhirClient) : IFhirExplorerService
{
    private static readonly HashSet<string> AllowedResources = new(StringComparer.OrdinalIgnoreCase)
    {
        "Account",
        "ActivityDefinition",
        "AllergyIntolerance",
        "Appointment",
        "AuditEvent",
        "Binary",
        "BodyStructure",
        "Bundle",
        "CapabilityStatement",
        "CarePlan",
        "CareTeam",
        "ChargeItem",
        "Claim",
        "ClaimResponse",
        "ClinicalImpression",
        "CodeSystem",
        "Communication",
        "CommunicationRequest",
        "CompartmentDefinition",
        "Composition",
        "ConceptMap",
        "Condition",
        "Consent",
        "Coverage",
        "CoverageEligibilityRequest",
        "CoverageEligibilityResponse",
        "DetectedIssue",
        "Device",
        "DiagnosticReport",
        "DocumentReference",
        "DomainResource",
        "Encounter",
        "Endpoint",
        "EpisodeOfCare",
        "Evidence",
        "EvidenceVariable",
        "ExplanationOfBenefit",
        "FamilyMemberHistory",
        "Flag",
        "Goal",
        "GuidanceResponse",
        "HealthcareService",
        "ImplementationGuide",
        "ImagingStudy",
        "Immunization",
        "Invoice",
        "Library",
        "Location",
        "Measure",
        "MeasureReport",
        "Medication",
        "MedicationAdministration",
        "MedicationDispense",
        "MedicationRequest",
        "MedicationStatement",
        "MessageHeader",
        "MolecularSequence",
        "NamingSystem",
        "Observation",
        "OperationDefinition",
        "OperationOutcome",
        "Organization",
        "Parameters",
        "Patient",
        "PaymentNotice",
        "PaymentReconciliation",
        "PlanDefinition",
        "Practitioner",
        "PractitionerRole",
        "Procedure",
        "Provenance",
        "Questionnaire",
        "QuestionnaireResponse",
        "RelatedPerson",
        "ResearchDefinition",
        "ResearchElementDefinition",
        "Resource",
        "RiskAssessment",
        "SearchParameter",
        "ServiceRequest",
        "Slot",
        "Specimen",
        "StructureDefinition",
        "Subscription",
        "Task",
        "ValueSet"
    };
    public async Task<FhirExplorerResponse> ExecuteAsync(FhirExplorerRequest request, CancellationToken cancellationToken)
    {
        if (!AllowedResources.Contains(request.ResourceType)) throw new InvalidOperationException($"Unsupported resource type '{request.ResourceType}'.");
        var resource = request.Interaction.Equals("read", StringComparison.OrdinalIgnoreCase)
            ? await fhirClient.ReadAsync(request.ResourceType, request.Id ?? throw new InvalidOperationException("Read requires an id."), cancellationToken)
            : await fhirClient.SearchAsync(request.ResourceType, request.Parameters, cancellationToken);
        var isXml = request.Format.Equals("xml", StringComparison.OrdinalIgnoreCase);
        var body = resource is null ? string.Empty : Serialize(resource, isXml);
        return new FhirExplorerResponse(200, request.ResourceType, request.Interaction, isXml ? "application/fhir+xml" : "application/fhir+json", body, new Dictionary<string, string>());
    }

    private static string Serialize(Resource resource, bool isXml) =>
        isXml ? resource.ToXml(pretty: true) : resource.ToJson(pretty: true);
}
