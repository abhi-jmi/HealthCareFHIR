using Microsoft.EntityFrameworkCore;

namespace FhirPlatform.Infrastructure;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ExtensionRegistryEntry> ExtensionRegistry => Set<ExtensionRegistryEntry>();
    public DbSet<SavedApiRequest> SavedApiRequests => Set<SavedApiRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExtensionRegistryEntry>().HasIndex(x => x.CanonicalUrl).IsUnique();
        modelBuilder.Entity<SavedApiRequest>().Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
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
