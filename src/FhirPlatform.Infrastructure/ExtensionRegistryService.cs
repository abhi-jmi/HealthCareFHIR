using FhirPlatform.Application;
using FhirPlatform.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FhirPlatform.Infrastructure;

public sealed class ExtensionRegistryService(AppDbContext dbContext) : IExtensionRegistryService
{
    public async Task<IReadOnlySet<string>> GetActiveCanonicalUrlsAsync(string resourceType, CancellationToken cancellationToken)
    {
        var entries = await dbContext.ExtensionRegistry.AsNoTracking().Where(e => e.Active).ToListAsync(cancellationToken);
        return entries
            .Where(e => AppliesTo(e, resourceType))
            .Select(e => e.CanonicalUrl)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public async Task<IReadOnlyList<ExtensionRegistryEntryDto>> ListAsync(CancellationToken cancellationToken) =>
        await dbContext.ExtensionRegistry.AsNoTracking().OrderBy(e => e.Name).Select(e => ToDto(e)).ToListAsync(cancellationToken);

    public async Task<ExtensionRegistryEntryDto?> GetAsync(Guid id, CancellationToken cancellationToken) =>
        await dbContext.ExtensionRegistry.AsNoTracking().Where(e => e.Id == id).Select(e => ToDto(e)).SingleOrDefaultAsync(cancellationToken);

    public async Task<ExtensionRegistryEntryDto> CreateAsync(UpsertExtensionRegistryEntryRequest request, CancellationToken cancellationToken)
    {
        var entity = new ExtensionRegistryEntry { Id = Guid.NewGuid(), CanonicalUrl = request.CanonicalUrl, Name = request.Name, Description = request.Description, ApplicableResourceTypes = string.Join(',', request.ApplicableResourceTypes), ValueType = request.ValueType, Active = request.Active };
        dbContext.ExtensionRegistry.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToDto(entity);
    }

    public async Task<ExtensionRegistryEntryDto?> UpdateAsync(Guid id, UpsertExtensionRegistryEntryRequest request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.ExtensionRegistry.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity is null) return null;
        entity.CanonicalUrl = request.CanonicalUrl;
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.ApplicableResourceTypes = string.Join(',', request.ApplicableResourceTypes);
        entity.ValueType = request.ValueType;
        entity.Active = request.Active;
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToDto(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await dbContext.ExtensionRegistry.Where(e => e.Id == id).ExecuteDeleteAsync(cancellationToken);
        return deleted > 0;
    }

    private static bool AppliesTo(ExtensionRegistryEntry entry, string resourceType) =>
        entry.ApplicableResourceTypes.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Any(value => value == "*" || value.Equals(resourceType, StringComparison.OrdinalIgnoreCase));

    private static ExtensionRegistryEntryDto ToDto(ExtensionRegistryEntry entry) => new(
        entry.Id,
        entry.CanonicalUrl,
        entry.Name,
        entry.Description,
        entry.ApplicableResourceTypes.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
        entry.ValueType,
        entry.Active);
}
