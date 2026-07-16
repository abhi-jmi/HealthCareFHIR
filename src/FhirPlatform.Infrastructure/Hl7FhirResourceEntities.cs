using System.ComponentModel.DataAnnotations;

namespace FhirPlatform.Infrastructure;

/// <summary>Relational projection of an HL7 FHIR resource for optional local schema visibility; Microsoft FHIR Server remains canonical.</summary>
public abstract class Hl7FhirResourceEntity
{
    public Guid Id { get; set; }
    [MaxLength(128)] public required string ResourceType { get; set; }
    [MaxLength(256)] public string? LogicalId { get; set; }
    [MaxLength(128)] public string? VersionId { get; set; }
    public DateTimeOffset? LastUpdated { get; set; }
    [MaxLength(256)] public string? Identifier { get; set; }
    [MaxLength(256)] public string? SubjectReference { get; set; }
    [MaxLength(256)] public string? PatientReference { get; set; }
    [MaxLength(128)] public string? Status { get; set; }
    [MaxLength(512)] public string? Display { get; set; }
    public string? CanonicalUrl { get; set; }
    public string? ProfileUrls { get; set; }
    public string? SecurityLabels { get; set; }
    public string? Tags { get; set; }
    public string? RawJson { get; set; }
    public string? RawXml { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class ResourceFhirEntity : Hl7FhirResourceEntity
{
    public ResourceFhirEntity() => ResourceType = "Resource";
}

public sealed class DomainResourceFhirEntity : Hl7FhirResourceEntity
{
    public DomainResourceFhirEntity() => ResourceType = "DomainResource";
}

public sealed class BundleFhirEntity : Hl7FhirResourceEntity
{
    public BundleFhirEntity() => ResourceType = "Bundle";
}

public sealed class OperationOutcomeFhirEntity : Hl7FhirResourceEntity
{
    public OperationOutcomeFhirEntity() => ResourceType = "OperationOutcome";
}

public sealed class BinaryFhirEntity : Hl7FhirResourceEntity
{
    public BinaryFhirEntity() => ResourceType = "Binary";
}

public sealed class ParametersFhirEntity : Hl7FhirResourceEntity
{
    public ParametersFhirEntity() => ResourceType = "Parameters";
}

public sealed class ImplementationGuideFhirEntity : Hl7FhirResourceEntity
{
    public ImplementationGuideFhirEntity() => ResourceType = "ImplementationGuide";
}

public sealed class SearchParameterFhirEntity : Hl7FhirResourceEntity
{
    public SearchParameterFhirEntity() => ResourceType = "SearchParameter";
}

public sealed class OperationDefinitionFhirEntity : Hl7FhirResourceEntity
{
    public OperationDefinitionFhirEntity() => ResourceType = "OperationDefinition";
}

public sealed class CompartmentDefinitionFhirEntity : Hl7FhirResourceEntity
{
    public CompartmentDefinitionFhirEntity() => ResourceType = "CompartmentDefinition";
}

public sealed class StructureDefinitionFhirEntity : Hl7FhirResourceEntity
{
    public StructureDefinitionFhirEntity() => ResourceType = "StructureDefinition";
}

public sealed class ConsentFhirEntity : Hl7FhirResourceEntity
{
    public ConsentFhirEntity() => ResourceType = "Consent";
}

public sealed class ProvenanceFhirEntity : Hl7FhirResourceEntity
{
    public ProvenanceFhirEntity() => ResourceType = "Provenance";
}

public sealed class AuditEventFhirEntity : Hl7FhirResourceEntity
{
    public AuditEventFhirEntity() => ResourceType = "AuditEvent";
}

public sealed class CapabilityStatementFhirEntity : Hl7FhirResourceEntity
{
    public CapabilityStatementFhirEntity() => ResourceType = "CapabilityStatement";
}

public sealed class CodeSystemFhirEntity : Hl7FhirResourceEntity
{
    public CodeSystemFhirEntity() => ResourceType = "CodeSystem";
}

public sealed class ValueSetFhirEntity : Hl7FhirResourceEntity
{
    public ValueSetFhirEntity() => ResourceType = "ValueSet";
}

public sealed class ConceptMapFhirEntity : Hl7FhirResourceEntity
{
    public ConceptMapFhirEntity() => ResourceType = "ConceptMap";
}

public sealed class NamingSystemFhirEntity : Hl7FhirResourceEntity
{
    public NamingSystemFhirEntity() => ResourceType = "NamingSystem";
}

public sealed class DocumentReferenceFhirEntity : Hl7FhirResourceEntity
{
    public DocumentReferenceFhirEntity() => ResourceType = "DocumentReference";
}

public sealed class MessageHeaderFhirEntity : Hl7FhirResourceEntity
{
    public MessageHeaderFhirEntity() => ResourceType = "MessageHeader";
}

public sealed class SubscriptionFhirEntity : Hl7FhirResourceEntity
{
    public SubscriptionFhirEntity() => ResourceType = "Subscription";
}

public sealed class TaskFhirEntity : Hl7FhirResourceEntity
{
    public TaskFhirEntity() => ResourceType = "Task";
}

public sealed class PatientFhirEntity : Hl7FhirResourceEntity
{
    public PatientFhirEntity() => ResourceType = "Patient";
}

public sealed class PractitionerFhirEntity : Hl7FhirResourceEntity
{
    public PractitionerFhirEntity() => ResourceType = "Practitioner";
}

public sealed class PractitionerRoleFhirEntity : Hl7FhirResourceEntity
{
    public PractitionerRoleFhirEntity() => ResourceType = "PractitionerRole";
}

public sealed class CareTeamFhirEntity : Hl7FhirResourceEntity
{
    public CareTeamFhirEntity() => ResourceType = "CareTeam";
}

public sealed class DeviceFhirEntity : Hl7FhirResourceEntity
{
    public DeviceFhirEntity() => ResourceType = "Device";
}

public sealed class OrganizationFhirEntity : Hl7FhirResourceEntity
{
    public OrganizationFhirEntity() => ResourceType = "Organization";
}

public sealed class LocationFhirEntity : Hl7FhirResourceEntity
{
    public LocationFhirEntity() => ResourceType = "Location";
}

public sealed class HealthcareServiceFhirEntity : Hl7FhirResourceEntity
{
    public HealthcareServiceFhirEntity() => ResourceType = "HealthcareService";
}

public sealed class EndpointFhirEntity : Hl7FhirResourceEntity
{
    public EndpointFhirEntity() => ResourceType = "Endpoint";
}

public sealed class RelatedPersonFhirEntity : Hl7FhirResourceEntity
{
    public RelatedPersonFhirEntity() => ResourceType = "RelatedPerson";
}

public sealed class AllergyIntoleranceFhirEntity : Hl7FhirResourceEntity
{
    public AllergyIntoleranceFhirEntity() => ResourceType = "AllergyIntolerance";
}

public sealed class ConditionFhirEntity : Hl7FhirResourceEntity
{
    public ConditionFhirEntity() => ResourceType = "Condition";
}

public sealed class ProcedureFhirEntity : Hl7FhirResourceEntity
{
    public ProcedureFhirEntity() => ResourceType = "Procedure";
}

public sealed class CarePlanFhirEntity : Hl7FhirResourceEntity
{
    public CarePlanFhirEntity() => ResourceType = "CarePlan";
}

public sealed class GoalFhirEntity : Hl7FhirResourceEntity
{
    public GoalFhirEntity() => ResourceType = "Goal";
}

public sealed class ServiceRequestFhirEntity : Hl7FhirResourceEntity
{
    public ServiceRequestFhirEntity() => ResourceType = "ServiceRequest";
}

public sealed class FamilyMemberHistoryFhirEntity : Hl7FhirResourceEntity
{
    public FamilyMemberHistoryFhirEntity() => ResourceType = "FamilyMemberHistory";
}

public sealed class RiskAssessmentFhirEntity : Hl7FhirResourceEntity
{
    public RiskAssessmentFhirEntity() => ResourceType = "RiskAssessment";
}

public sealed class ClinicalImpressionFhirEntity : Hl7FhirResourceEntity
{
    public ClinicalImpressionFhirEntity() => ResourceType = "ClinicalImpression";
}

public sealed class CompositionFhirEntity : Hl7FhirResourceEntity
{
    public CompositionFhirEntity() => ResourceType = "Composition";
}

public sealed class QuestionnaireFhirEntity : Hl7FhirResourceEntity
{
    public QuestionnaireFhirEntity() => ResourceType = "Questionnaire";
}

public sealed class QuestionnaireResponseFhirEntity : Hl7FhirResourceEntity
{
    public QuestionnaireResponseFhirEntity() => ResourceType = "QuestionnaireResponse";
}

public sealed class ObservationFhirEntity : Hl7FhirResourceEntity
{
    public ObservationFhirEntity() => ResourceType = "Observation";
}

public sealed class DiagnosticReportFhirEntity : Hl7FhirResourceEntity
{
    public DiagnosticReportFhirEntity() => ResourceType = "DiagnosticReport";
}

public sealed class SpecimenFhirEntity : Hl7FhirResourceEntity
{
    public SpecimenFhirEntity() => ResourceType = "Specimen";
}

public sealed class ImagingStudyFhirEntity : Hl7FhirResourceEntity
{
    public ImagingStudyFhirEntity() => ResourceType = "ImagingStudy";
}

public sealed class MediaFhirEntity : Hl7FhirResourceEntity
{
    public MediaFhirEntity() => ResourceType = "Media";
}

public sealed class BodyStructureFhirEntity : Hl7FhirResourceEntity
{
    public BodyStructureFhirEntity() => ResourceType = "BodyStructure";
}

public sealed class MolecularSequenceFhirEntity : Hl7FhirResourceEntity
{
    public MolecularSequenceFhirEntity() => ResourceType = "MolecularSequence";
}

public sealed class MedicationFhirEntity : Hl7FhirResourceEntity
{
    public MedicationFhirEntity() => ResourceType = "Medication";
}

public sealed class MedicationRequestFhirEntity : Hl7FhirResourceEntity
{
    public MedicationRequestFhirEntity() => ResourceType = "MedicationRequest";
}

public sealed class MedicationDispenseFhirEntity : Hl7FhirResourceEntity
{
    public MedicationDispenseFhirEntity() => ResourceType = "MedicationDispense";
}

public sealed class MedicationAdministrationFhirEntity : Hl7FhirResourceEntity
{
    public MedicationAdministrationFhirEntity() => ResourceType = "MedicationAdministration";
}

public sealed class MedicationStatementFhirEntity : Hl7FhirResourceEntity
{
    public MedicationStatementFhirEntity() => ResourceType = "MedicationStatement";
}

public sealed class AppointmentFhirEntity : Hl7FhirResourceEntity
{
    public AppointmentFhirEntity() => ResourceType = "Appointment";
}

public sealed class ScheduleFhirEntity : Hl7FhirResourceEntity
{
    public ScheduleFhirEntity() => ResourceType = "Schedule";
}

public sealed class SlotFhirEntity : Hl7FhirResourceEntity
{
    public SlotFhirEntity() => ResourceType = "Slot";
}

public sealed class CommunicationFhirEntity : Hl7FhirResourceEntity
{
    public CommunicationFhirEntity() => ResourceType = "Communication";
}

public sealed class CommunicationRequestFhirEntity : Hl7FhirResourceEntity
{
    public CommunicationRequestFhirEntity() => ResourceType = "CommunicationRequest";
}

public sealed class EpisodeOfCareFhirEntity : Hl7FhirResourceEntity
{
    public EpisodeOfCareFhirEntity() => ResourceType = "EpisodeOfCare";
}

public sealed class CoverageFhirEntity : Hl7FhirResourceEntity
{
    public CoverageFhirEntity() => ResourceType = "Coverage";
}

public sealed class ClaimFhirEntity : Hl7FhirResourceEntity
{
    public ClaimFhirEntity() => ResourceType = "Claim";
}

public sealed class ClaimResponseFhirEntity : Hl7FhirResourceEntity
{
    public ClaimResponseFhirEntity() => ResourceType = "ClaimResponse";
}

public sealed class ExplanationOfBenefitFhirEntity : Hl7FhirResourceEntity
{
    public ExplanationOfBenefitFhirEntity() => ResourceType = "ExplanationOfBenefit";
}

public sealed class InvoiceFhirEntity : Hl7FhirResourceEntity
{
    public InvoiceFhirEntity() => ResourceType = "Invoice";
}

public sealed class AccountFhirEntity : Hl7FhirResourceEntity
{
    public AccountFhirEntity() => ResourceType = "Account";
}

public sealed class ChargeItemFhirEntity : Hl7FhirResourceEntity
{
    public ChargeItemFhirEntity() => ResourceType = "ChargeItem";
}

public sealed class PaymentNoticeFhirEntity : Hl7FhirResourceEntity
{
    public PaymentNoticeFhirEntity() => ResourceType = "PaymentNotice";
}

public sealed class PaymentReconciliationFhirEntity : Hl7FhirResourceEntity
{
    public PaymentReconciliationFhirEntity() => ResourceType = "PaymentReconciliation";
}

public sealed class LibraryFhirEntity : Hl7FhirResourceEntity
{
    public LibraryFhirEntity() => ResourceType = "Library";
}

public sealed class PlanDefinitionFhirEntity : Hl7FhirResourceEntity
{
    public PlanDefinitionFhirEntity() => ResourceType = "PlanDefinition";
}

public sealed class ActivityDefinitionFhirEntity : Hl7FhirResourceEntity
{
    public ActivityDefinitionFhirEntity() => ResourceType = "ActivityDefinition";
}

public sealed class GuidanceResponseFhirEntity : Hl7FhirResourceEntity
{
    public GuidanceResponseFhirEntity() => ResourceType = "GuidanceResponse";
}

public sealed class MeasureFhirEntity : Hl7FhirResourceEntity
{
    public MeasureFhirEntity() => ResourceType = "Measure";
}

public sealed class MeasureReportFhirEntity : Hl7FhirResourceEntity
{
    public MeasureReportFhirEntity() => ResourceType = "MeasureReport";
}

public sealed class EvidenceFhirEntity : Hl7FhirResourceEntity
{
    public EvidenceFhirEntity() => ResourceType = "Evidence";
}

public sealed class EvidenceVariableFhirEntity : Hl7FhirResourceEntity
{
    public EvidenceVariableFhirEntity() => ResourceType = "EvidenceVariable";
}

public sealed class ResearchDefinitionFhirEntity : Hl7FhirResourceEntity
{
    public ResearchDefinitionFhirEntity() => ResourceType = "ResearchDefinition";
}

public sealed class ResearchElementDefinitionFhirEntity : Hl7FhirResourceEntity
{
    public ResearchElementDefinitionFhirEntity() => ResourceType = "ResearchElementDefinition";
}

public static class Hl7FhirSchemaCatalog
{
    public static readonly IReadOnlyDictionary<Type, string> Tables = new Dictionary<Type, string>
    {
        [typeof(ResourceFhirEntity)] = "Hl7FhirResource",
        [typeof(DomainResourceFhirEntity)] = "Hl7FhirDomainResource",
        [typeof(BundleFhirEntity)] = "Hl7FhirBundle",
        [typeof(OperationOutcomeFhirEntity)] = "Hl7FhirOperationOutcome",
        [typeof(BinaryFhirEntity)] = "Hl7FhirBinary",
        [typeof(ParametersFhirEntity)] = "Hl7FhirParameters",
        [typeof(ImplementationGuideFhirEntity)] = "Hl7FhirImplementationGuide",
        [typeof(SearchParameterFhirEntity)] = "Hl7FhirSearchParameter",
        [typeof(OperationDefinitionFhirEntity)] = "Hl7FhirOperationDefinition",
        [typeof(CompartmentDefinitionFhirEntity)] = "Hl7FhirCompartmentDefinition",
        [typeof(StructureDefinitionFhirEntity)] = "Hl7FhirStructureDefinition",
        [typeof(ConsentFhirEntity)] = "Hl7FhirConsent",
        [typeof(ProvenanceFhirEntity)] = "Hl7FhirProvenance",
        [typeof(AuditEventFhirEntity)] = "Hl7FhirAuditEvent",
        [typeof(CapabilityStatementFhirEntity)] = "Hl7FhirCapabilityStatement",
        [typeof(CodeSystemFhirEntity)] = "Hl7FhirCodeSystem",
        [typeof(ValueSetFhirEntity)] = "Hl7FhirValueSet",
        [typeof(ConceptMapFhirEntity)] = "Hl7FhirConceptMap",
        [typeof(NamingSystemFhirEntity)] = "Hl7FhirNamingSystem",
        [typeof(DocumentReferenceFhirEntity)] = "Hl7FhirDocumentReference",
        [typeof(MessageHeaderFhirEntity)] = "Hl7FhirMessageHeader",
        [typeof(SubscriptionFhirEntity)] = "Hl7FhirSubscription",
        [typeof(TaskFhirEntity)] = "Hl7FhirTask",
        [typeof(PatientFhirEntity)] = "Hl7FhirPatient",
        [typeof(PractitionerFhirEntity)] = "Hl7FhirPractitioner",
        [typeof(PractitionerRoleFhirEntity)] = "Hl7FhirPractitionerRole",
        [typeof(CareTeamFhirEntity)] = "Hl7FhirCareTeam",
        [typeof(DeviceFhirEntity)] = "Hl7FhirDevice",
        [typeof(OrganizationFhirEntity)] = "Hl7FhirOrganization",
        [typeof(LocationFhirEntity)] = "Hl7FhirLocation",
        [typeof(HealthcareServiceFhirEntity)] = "Hl7FhirHealthcareService",
        [typeof(EndpointFhirEntity)] = "Hl7FhirEndpoint",
        [typeof(RelatedPersonFhirEntity)] = "Hl7FhirRelatedPerson",
        [typeof(AllergyIntoleranceFhirEntity)] = "Hl7FhirAllergyIntolerance",
        [typeof(ConditionFhirEntity)] = "Hl7FhirCondition",
        [typeof(ProcedureFhirEntity)] = "Hl7FhirProcedure",
        [typeof(CarePlanFhirEntity)] = "Hl7FhirCarePlan",
        [typeof(GoalFhirEntity)] = "Hl7FhirGoal",
        [typeof(ServiceRequestFhirEntity)] = "Hl7FhirServiceRequest",
        [typeof(FamilyMemberHistoryFhirEntity)] = "Hl7FhirFamilyMemberHistory",
        [typeof(RiskAssessmentFhirEntity)] = "Hl7FhirRiskAssessment",
        [typeof(ClinicalImpressionFhirEntity)] = "Hl7FhirClinicalImpression",
        [typeof(CompositionFhirEntity)] = "Hl7FhirComposition",
        [typeof(QuestionnaireFhirEntity)] = "Hl7FhirQuestionnaire",
        [typeof(QuestionnaireResponseFhirEntity)] = "Hl7FhirQuestionnaireResponse",
        [typeof(ObservationFhirEntity)] = "Hl7FhirObservation",
        [typeof(DiagnosticReportFhirEntity)] = "Hl7FhirDiagnosticReport",
        [typeof(SpecimenFhirEntity)] = "Hl7FhirSpecimen",
        [typeof(ImagingStudyFhirEntity)] = "Hl7FhirImagingStudy",
        [typeof(MediaFhirEntity)] = "Hl7FhirMedia",
        [typeof(BodyStructureFhirEntity)] = "Hl7FhirBodyStructure",
        [typeof(MolecularSequenceFhirEntity)] = "Hl7FhirMolecularSequence",
        [typeof(MedicationFhirEntity)] = "Hl7FhirMedication",
        [typeof(MedicationRequestFhirEntity)] = "Hl7FhirMedicationRequest",
        [typeof(MedicationDispenseFhirEntity)] = "Hl7FhirMedicationDispense",
        [typeof(MedicationAdministrationFhirEntity)] = "Hl7FhirMedicationAdministration",
        [typeof(MedicationStatementFhirEntity)] = "Hl7FhirMedicationStatement",
        [typeof(AppointmentFhirEntity)] = "Hl7FhirAppointment",
        [typeof(ScheduleFhirEntity)] = "Hl7FhirSchedule",
        [typeof(SlotFhirEntity)] = "Hl7FhirSlot",
        [typeof(CommunicationFhirEntity)] = "Hl7FhirCommunication",
        [typeof(CommunicationRequestFhirEntity)] = "Hl7FhirCommunicationRequest",
        [typeof(EpisodeOfCareFhirEntity)] = "Hl7FhirEpisodeOfCare",
        [typeof(CoverageFhirEntity)] = "Hl7FhirCoverage",
        [typeof(ClaimFhirEntity)] = "Hl7FhirClaim",
        [typeof(ClaimResponseFhirEntity)] = "Hl7FhirClaimResponse",
        [typeof(ExplanationOfBenefitFhirEntity)] = "Hl7FhirExplanationOfBenefit",
        [typeof(InvoiceFhirEntity)] = "Hl7FhirInvoice",
        [typeof(AccountFhirEntity)] = "Hl7FhirAccount",
        [typeof(ChargeItemFhirEntity)] = "Hl7FhirChargeItem",
        [typeof(PaymentNoticeFhirEntity)] = "Hl7FhirPaymentNotice",
        [typeof(PaymentReconciliationFhirEntity)] = "Hl7FhirPaymentReconciliation",
        [typeof(LibraryFhirEntity)] = "Hl7FhirLibrary",
        [typeof(PlanDefinitionFhirEntity)] = "Hl7FhirPlanDefinition",
        [typeof(ActivityDefinitionFhirEntity)] = "Hl7FhirActivityDefinition",
        [typeof(GuidanceResponseFhirEntity)] = "Hl7FhirGuidanceResponse",
        [typeof(MeasureFhirEntity)] = "Hl7FhirMeasure",
        [typeof(MeasureReportFhirEntity)] = "Hl7FhirMeasureReport",
        [typeof(EvidenceFhirEntity)] = "Hl7FhirEvidence",
        [typeof(EvidenceVariableFhirEntity)] = "Hl7FhirEvidenceVariable",
        [typeof(ResearchDefinitionFhirEntity)] = "Hl7FhirResearchDefinition",
        [typeof(ResearchElementDefinitionFhirEntity)] = "Hl7FhirResearchElementDefinition",
    };
}