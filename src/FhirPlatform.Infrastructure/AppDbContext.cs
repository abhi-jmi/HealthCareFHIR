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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExtensionRegistryEntry>().HasIndex(x => x.CanonicalUrl).IsUnique();
        modelBuilder.Entity<SavedApiRequest>().Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        modelBuilder.Entity<OperationalAuditCorrelation>().HasIndex(x => x.CorrelationId).IsUnique();
        modelBuilder.Entity<IngestionJob>().HasIndex(x => x.IdempotencyKey).IsUnique();
        modelBuilder.Entity<RuleConfiguration>().HasIndex(x => new { x.RuleId, x.Version }).IsUnique();
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
