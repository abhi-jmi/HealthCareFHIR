using Microsoft.EntityFrameworkCore;

namespace FhirPlatform.Infrastructure;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ExtensionRegistryEntry> ExtensionRegistry => Set<ExtensionRegistryEntry>();
    public DbSet<SavedApiRequest> SavedApiRequests => Set<SavedApiRequest>();
    public DbSet<OperationalAuditCorrelation> OperationalAuditCorrelations => Set<OperationalAuditCorrelation>();
    public DbSet<IngestionJob> IngestionJobs => Set<IngestionJob>();
    public DbSet<RuleConfiguration> RuleConfigurations => Set<RuleConfiguration>();
    public DbSet<RuleExecutionAudit> RuleExecutionAudits => Set<RuleExecutionAudit>();
    public DbSet<ResourceFhirEntity> Hl7FhirResource => Set<ResourceFhirEntity>();
    public DbSet<DomainResourceFhirEntity> Hl7FhirDomainResource => Set<DomainResourceFhirEntity>();
    public DbSet<BundleFhirEntity> Hl7FhirBundle => Set<BundleFhirEntity>();
    public DbSet<OperationOutcomeFhirEntity> Hl7FhirOperationOutcome => Set<OperationOutcomeFhirEntity>();
    public DbSet<BinaryFhirEntity> Hl7FhirBinary => Set<BinaryFhirEntity>();
    public DbSet<ParametersFhirEntity> Hl7FhirParameters => Set<ParametersFhirEntity>();
    public DbSet<ImplementationGuideFhirEntity> Hl7FhirImplementationGuide => Set<ImplementationGuideFhirEntity>();
    public DbSet<SearchParameterFhirEntity> Hl7FhirSearchParameter => Set<SearchParameterFhirEntity>();
    public DbSet<OperationDefinitionFhirEntity> Hl7FhirOperationDefinition => Set<OperationDefinitionFhirEntity>();
    public DbSet<CompartmentDefinitionFhirEntity> Hl7FhirCompartmentDefinition => Set<CompartmentDefinitionFhirEntity>();
    public DbSet<StructureDefinitionFhirEntity> Hl7FhirStructureDefinition => Set<StructureDefinitionFhirEntity>();
    public DbSet<ConsentFhirEntity> Hl7FhirConsent => Set<ConsentFhirEntity>();
    public DbSet<ProvenanceFhirEntity> Hl7FhirProvenance => Set<ProvenanceFhirEntity>();
    public DbSet<AuditEventFhirEntity> Hl7FhirAuditEvent => Set<AuditEventFhirEntity>();
    public DbSet<CapabilityStatementFhirEntity> Hl7FhirCapabilityStatement => Set<CapabilityStatementFhirEntity>();
    public DbSet<CodeSystemFhirEntity> Hl7FhirCodeSystem => Set<CodeSystemFhirEntity>();
    public DbSet<ValueSetFhirEntity> Hl7FhirValueSet => Set<ValueSetFhirEntity>();
    public DbSet<ConceptMapFhirEntity> Hl7FhirConceptMap => Set<ConceptMapFhirEntity>();
    public DbSet<NamingSystemFhirEntity> Hl7FhirNamingSystem => Set<NamingSystemFhirEntity>();
    public DbSet<DocumentReferenceFhirEntity> Hl7FhirDocumentReference => Set<DocumentReferenceFhirEntity>();
    public DbSet<MessageHeaderFhirEntity> Hl7FhirMessageHeader => Set<MessageHeaderFhirEntity>();
    public DbSet<SubscriptionFhirEntity> Hl7FhirSubscription => Set<SubscriptionFhirEntity>();
    public DbSet<TaskFhirEntity> Hl7FhirTask => Set<TaskFhirEntity>();
    public DbSet<PatientFhirEntity> Hl7FhirPatient => Set<PatientFhirEntity>();
    public DbSet<PractitionerFhirEntity> Hl7FhirPractitioner => Set<PractitionerFhirEntity>();
    public DbSet<PractitionerRoleFhirEntity> Hl7FhirPractitionerRole => Set<PractitionerRoleFhirEntity>();
    public DbSet<CareTeamFhirEntity> Hl7FhirCareTeam => Set<CareTeamFhirEntity>();
    public DbSet<DeviceFhirEntity> Hl7FhirDevice => Set<DeviceFhirEntity>();
    public DbSet<OrganizationFhirEntity> Hl7FhirOrganization => Set<OrganizationFhirEntity>();
    public DbSet<LocationFhirEntity> Hl7FhirLocation => Set<LocationFhirEntity>();
    public DbSet<HealthcareServiceFhirEntity> Hl7FhirHealthcareService => Set<HealthcareServiceFhirEntity>();
    public DbSet<EndpointFhirEntity> Hl7FhirEndpoint => Set<EndpointFhirEntity>();
    public DbSet<RelatedPersonFhirEntity> Hl7FhirRelatedPerson => Set<RelatedPersonFhirEntity>();
    public DbSet<AllergyIntoleranceFhirEntity> Hl7FhirAllergyIntolerance => Set<AllergyIntoleranceFhirEntity>();
    public DbSet<ConditionFhirEntity> Hl7FhirCondition => Set<ConditionFhirEntity>();
    public DbSet<ProcedureFhirEntity> Hl7FhirProcedure => Set<ProcedureFhirEntity>();
    public DbSet<CarePlanFhirEntity> Hl7FhirCarePlan => Set<CarePlanFhirEntity>();
    public DbSet<GoalFhirEntity> Hl7FhirGoal => Set<GoalFhirEntity>();
    public DbSet<ServiceRequestFhirEntity> Hl7FhirServiceRequest => Set<ServiceRequestFhirEntity>();
    public DbSet<FamilyMemberHistoryFhirEntity> Hl7FhirFamilyMemberHistory => Set<FamilyMemberHistoryFhirEntity>();
    public DbSet<RiskAssessmentFhirEntity> Hl7FhirRiskAssessment => Set<RiskAssessmentFhirEntity>();
    public DbSet<ClinicalImpressionFhirEntity> Hl7FhirClinicalImpression => Set<ClinicalImpressionFhirEntity>();
    public DbSet<CompositionFhirEntity> Hl7FhirComposition => Set<CompositionFhirEntity>();
    public DbSet<QuestionnaireFhirEntity> Hl7FhirQuestionnaire => Set<QuestionnaireFhirEntity>();
    public DbSet<QuestionnaireResponseFhirEntity> Hl7FhirQuestionnaireResponse => Set<QuestionnaireResponseFhirEntity>();
    public DbSet<ObservationFhirEntity> Hl7FhirObservation => Set<ObservationFhirEntity>();
    public DbSet<DiagnosticReportFhirEntity> Hl7FhirDiagnosticReport => Set<DiagnosticReportFhirEntity>();
    public DbSet<SpecimenFhirEntity> Hl7FhirSpecimen => Set<SpecimenFhirEntity>();
    public DbSet<ImagingStudyFhirEntity> Hl7FhirImagingStudy => Set<ImagingStudyFhirEntity>();
    public DbSet<MediaFhirEntity> Hl7FhirMedia => Set<MediaFhirEntity>();
    public DbSet<BodyStructureFhirEntity> Hl7FhirBodyStructure => Set<BodyStructureFhirEntity>();
    public DbSet<MolecularSequenceFhirEntity> Hl7FhirMolecularSequence => Set<MolecularSequenceFhirEntity>();
    public DbSet<MedicationFhirEntity> Hl7FhirMedication => Set<MedicationFhirEntity>();
    public DbSet<MedicationRequestFhirEntity> Hl7FhirMedicationRequest => Set<MedicationRequestFhirEntity>();
    public DbSet<MedicationDispenseFhirEntity> Hl7FhirMedicationDispense => Set<MedicationDispenseFhirEntity>();
    public DbSet<MedicationAdministrationFhirEntity> Hl7FhirMedicationAdministration => Set<MedicationAdministrationFhirEntity>();
    public DbSet<MedicationStatementFhirEntity> Hl7FhirMedicationStatement => Set<MedicationStatementFhirEntity>();
    public DbSet<AppointmentFhirEntity> Hl7FhirAppointment => Set<AppointmentFhirEntity>();
    public DbSet<ScheduleFhirEntity> Hl7FhirSchedule => Set<ScheduleFhirEntity>();
    public DbSet<SlotFhirEntity> Hl7FhirSlot => Set<SlotFhirEntity>();
    public DbSet<CommunicationFhirEntity> Hl7FhirCommunication => Set<CommunicationFhirEntity>();
    public DbSet<CommunicationRequestFhirEntity> Hl7FhirCommunicationRequest => Set<CommunicationRequestFhirEntity>();
    public DbSet<EpisodeOfCareFhirEntity> Hl7FhirEpisodeOfCare => Set<EpisodeOfCareFhirEntity>();
    public DbSet<CoverageFhirEntity> Hl7FhirCoverage => Set<CoverageFhirEntity>();
    public DbSet<ClaimFhirEntity> Hl7FhirClaim => Set<ClaimFhirEntity>();
    public DbSet<ClaimResponseFhirEntity> Hl7FhirClaimResponse => Set<ClaimResponseFhirEntity>();
    public DbSet<ExplanationOfBenefitFhirEntity> Hl7FhirExplanationOfBenefit => Set<ExplanationOfBenefitFhirEntity>();
    public DbSet<InvoiceFhirEntity> Hl7FhirInvoice => Set<InvoiceFhirEntity>();
    public DbSet<AccountFhirEntity> Hl7FhirAccount => Set<AccountFhirEntity>();
    public DbSet<ChargeItemFhirEntity> Hl7FhirChargeItem => Set<ChargeItemFhirEntity>();
    public DbSet<PaymentNoticeFhirEntity> Hl7FhirPaymentNotice => Set<PaymentNoticeFhirEntity>();
    public DbSet<PaymentReconciliationFhirEntity> Hl7FhirPaymentReconciliation => Set<PaymentReconciliationFhirEntity>();
    public DbSet<LibraryFhirEntity> Hl7FhirLibrary => Set<LibraryFhirEntity>();
    public DbSet<PlanDefinitionFhirEntity> Hl7FhirPlanDefinition => Set<PlanDefinitionFhirEntity>();
    public DbSet<ActivityDefinitionFhirEntity> Hl7FhirActivityDefinition => Set<ActivityDefinitionFhirEntity>();
    public DbSet<GuidanceResponseFhirEntity> Hl7FhirGuidanceResponse => Set<GuidanceResponseFhirEntity>();
    public DbSet<MeasureFhirEntity> Hl7FhirMeasure => Set<MeasureFhirEntity>();
    public DbSet<MeasureReportFhirEntity> Hl7FhirMeasureReport => Set<MeasureReportFhirEntity>();
    public DbSet<EvidenceFhirEntity> Hl7FhirEvidence => Set<EvidenceFhirEntity>();
    public DbSet<EvidenceVariableFhirEntity> Hl7FhirEvidenceVariable => Set<EvidenceVariableFhirEntity>();
    public DbSet<ResearchDefinitionFhirEntity> Hl7FhirResearchDefinition => Set<ResearchDefinitionFhirEntity>();
    public DbSet<ResearchElementDefinitionFhirEntity> Hl7FhirResearchElementDefinition => Set<ResearchElementDefinitionFhirEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExtensionRegistryEntry>().HasIndex(x => x.CanonicalUrl).IsUnique();
        modelBuilder.Entity<SavedApiRequest>().Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        modelBuilder.Entity<OperationalAuditCorrelation>().HasIndex(x => x.CorrelationId).IsUnique();
        modelBuilder.Entity<IngestionJob>().HasIndex(x => x.IdempotencyKey).IsUnique();
        modelBuilder.Entity<RuleConfiguration>().HasIndex(x => new { x.RuleId, x.Version }).IsUnique();
        foreach (var table in Hl7FhirSchemaCatalog.Tables)
        {
            modelBuilder.Entity(table.Key).ToTable(table.Value);
            modelBuilder.Entity(table.Key).HasIndex(nameof(Hl7FhirResourceEntity.LogicalId));
            modelBuilder.Entity(table.Key).HasIndex(nameof(Hl7FhirResourceEntity.Identifier));
            modelBuilder.Entity(table.Key).HasIndex(nameof(Hl7FhirResourceEntity.SubjectReference));
            modelBuilder.Entity(table.Key).HasIndex(nameof(Hl7FhirResourceEntity.PatientReference));
        }
    }
}

public sealed class ExtensionRegistryEntry
{
    public Guid Id { get; set; }
    public required string CanonicalUrl { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string ApplicableResourceTypes { get; set; }
    public required string ValueType { get; set; }
    public bool Active { get; set; } = true;
}

public sealed class SavedApiRequest
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string ResourceType { get; set; }
    public required string Interaction { get; set; }
    public string? ParametersJson { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public sealed class OperationalAuditCorrelation
{
    public Guid Id { get; set; }
    public required string CorrelationId { get; set; }
    public required string EventType { get; set; }
    public string? FhirAuditEventReference { get; set; }
    public string? UserSubject { get; set; }
    public DateTimeOffset OccurredAt { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class IngestionJob
{
    public Guid Id { get; set; }
    public required string IdempotencyKey { get; set; }
    public required string Status { get; set; }
    public string? OperationOutcomeJson { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? CompletedAt { get; set; }
}


public sealed class RuleConfiguration
{
    public Guid Id { get; set; }
    public required string RuleId { get; set; }
    public required string Version { get; set; }
    public required string DisplayName { get; set; }
    public required string ConfigurationJson { get; set; }
    public bool Active { get; set; } = true;
}

public sealed class RuleExecutionAudit
{
    public Guid Id { get; set; }
    public required string PatientReference { get; set; }
    public required string RuleId { get; set; }
    public required string RuleVersion { get; set; }
    public required string InputResourceIds { get; set; }
    public DateTimeOffset ExecutedAt { get; set; } = DateTimeOffset.UtcNow;
    public required string Result { get; set; }
    public required string OutputResourceIds { get; set; }
    public required string InitiatedBy { get; set; }
}
