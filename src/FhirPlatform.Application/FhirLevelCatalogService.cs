using FhirPlatform.Application.Contracts;

namespace FhirPlatform.Application;

public interface IFhirLevelCatalogService
{
    FhirLevelCatalogDto GetCatalog();
}

public sealed class FhirLevelCatalogService : IFhirLevelCatalogService
{
    public FhirLevelCatalogDto GetCatalog() => new([
        new FhirLevelDto(1, "Foundation", "Basic framework on which the FHIR R4 specification is built.", [
            Module("foundation", "Foundation", "Base Documentation, XML, JSON, Data Types and Extensions.", ["Resource", "DomainResource", "Bundle", "OperationOutcome", "Binary", "Parameters"], ["/validation", "/administration/extensions"], ["/api/fhir/validation", "/api/administration/extensions"])
        ]),
        new FhirLevelDto(2, "Implementer Support", "Supporting implementation and binding to external specifications.", [
            Module("implementer-support", "Implementer Support", "Downloads, version management, use cases and operational support metadata.", ["ImplementationGuide", "SearchParameter", "OperationDefinition", "CompartmentDefinition", "StructureDefinition"], ["/conformance"], ["/api/conformance/dashboard/details"]),
            Module("security-privacy", "Security & Privacy", "Security, Consent, Provenance and AuditEvent.", ["Consent", "Provenance", "AuditEvent"], ["/audit", "/resources/Consent", "/resources/Provenance"], ["/api/audit", "/api/resources/Consent", "/api/resources/Provenance"]),
            Module("conformance", "Conformance", "StructureDefinition, CapabilityStatement, ImplementationGuide and profiling metadata.", ["CapabilityStatement", "StructureDefinition", "ImplementationGuide", "SearchParameter", "OperationDefinition", "CompartmentDefinition"], ["/conformance"], ["/api/conformance/dashboard/details"]),
            Module("terminology", "Terminology", "CodeSystem, ValueSet, ConceptMap and external terminology service operations.", ["CodeSystem", "ValueSet", "ConceptMap", "NamingSystem"], ["/terminology"], ["/api/terminology/code-systems", "/api/terminology/value-sets", "/api/terminology/concept-maps"]),
            Module("exchange", "Exchange", "REST API, search, documents, messaging, services and persistence through Microsoft FHIR Server.", ["Bundle", "DocumentReference", "MessageHeader", "Subscription", "Task"], ["/api-explorer", "/imports"], ["/api/fhir-explorer", "/api/ingestion/fhir-bundle"])
        ]),
        new FhirLevelDto(3, "Administration", "Linking to real-world concepts in the healthcare system.", [
            Module("administration", "Administration", "Patient, Practitioner, CareTeam, Device, Organization, Location and HealthcareService.", ["Patient", "Practitioner", "PractitionerRole", "CareTeam", "Device", "Organization", "Location", "HealthcareService", "Endpoint", "RelatedPerson"], ["/patients", "/resources/Practitioner", "/resources/Organization", "/resources/Location", "/resources/CareTeam", "/resources/Device", "/resources/HealthcareService"], ["/api/patients", "/api/resources/{resourceType}"])
        ]),
        new FhirLevelDto(4, "Record Keeping and Data Exchange", "FHIR resources for the healthcare process.", [
            Module("clinical", "Clinical", "Allergy, problem, procedure, care plan, goal, service request, family history and risk assessment.", ["AllergyIntolerance", "Condition", "Procedure", "CarePlan", "Goal", "ServiceRequest", "FamilyMemberHistory", "RiskAssessment", "ClinicalImpression", "DocumentReference", "Composition", "Questionnaire", "QuestionnaireResponse"], ["/resources/Condition", "/resources/AllergyIntolerance", "/resources/Procedure", "/resources/CarePlan", "/resources/Questionnaire"], ["/api/resources/{resourceType}"]),
            Module("diagnostics", "Diagnostics", "Observation, DiagnosticReport, Specimen, ImagingStudy, Media and genomics resources.", ["Observation", "DiagnosticReport", "Specimen", "ImagingStudy", "Media", "BodyStructure", "MolecularSequence"], ["/resources/Observation", "/resources/DiagnosticReport", "/imports"], ["/api/resources/{resourceType}", "/api/ingestion/fhir-bundle"]),
            Module("medications", "Medications", "Medication request, dispense, administration, statement, immunization and medication resources.", ["Medication", "MedicationRequest", "MedicationDispense", "MedicationAdministration", "MedicationStatement", "Immunization"], ["/resources/MedicationRequest", "/resources/MedicationStatement", "/resources/MedicationAdministration", "/resources/Immunization"], ["/api/resources/{resourceType}"]),
            Module("workflow", "Workflow", "Task, Appointment, Schedule, referral/service request and planning resources.", ["Task", "Appointment", "Schedule", "Slot", "ServiceRequest", "Communication", "CommunicationRequest", "EpisodeOfCare", "PlanDefinition"], ["/resources/Task", "/resources/Appointment", "/resources/Schedule", "/resources/ServiceRequest", "/resources/PlanDefinition"], ["/api/resources/{resourceType}"]),
            Module("financial", "Financial", "Claim, Account, Invoice, ChargeItem, Coverage, eligibility, EOB and payment resources.", ["Coverage", "CoverageEligibilityRequest", "CoverageEligibilityResponse", "Claim", "ClaimResponse", "ExplanationOfBenefit", "Invoice", "Account", "ChargeItem", "PaymentNotice", "PaymentReconciliation"], ["/resources/Coverage", "/resources/CoverageEligibilityRequest", "/resources/CoverageEligibilityResponse", "/resources/Claim", "/resources/ExplanationOfBenefit", "/resources/Invoice"], ["/api/resources/{resourceType}"])
        ]),
        new FhirLevelDto(5, "Clinical Reasoning", "Providing the ability to reason about the healthcare process using deterministic configured rules.", [
            Module("clinical-reasoning", "Clinical Reasoning", "Library, PlanDefinition, GuidanceResponse, Measure and MeasureReport.", ["Library", "PlanDefinition", "ActivityDefinition", "GuidanceResponse", "Measure", "MeasureReport", "Evidence", "EvidenceVariable", "ResearchDefinition", "ResearchElementDefinition"], ["/clinical-reasoning", "/resources/Library", "/resources/PlanDefinition", "/resources/Measure"], ["/api/clinical-reasoning", "/api/resources/{resourceType}"]),
            Module("medication-definition-r5", "Medication Definition (R5 alignment)", "FHIR R5 adds medication definition resources; this R4 platform tracks them as an upgrade planning area.", ["MedicinalProductDefinition", "PackagedProductDefinition", "AdministrableProductDefinition", "RegulatedAuthorization"], [], [])
        ])
    ]);

    private static FhirModuleDto Module(string key, string title, string summary, IReadOnlyList<string> resources, IReadOnlyList<string> uiRoutes, IReadOnlyList<string> apiRoutes) => new(key, title, summary, resources, uiRoutes, apiRoutes);
}
