namespace FhirPlatform.Application.Contracts;

public sealed record FhirLevelCatalogDto(IReadOnlyList<FhirLevelDto> Levels);
public sealed record FhirLevelDto(int Level, string Name, string Description, IReadOnlyList<FhirModuleDto> Modules);
public sealed record FhirModuleDto(string Key, string Title, string Summary, IReadOnlyList<string> ResourceTypes, IReadOnlyList<string> UiRoutes, IReadOnlyList<string> ApiRoutes);
