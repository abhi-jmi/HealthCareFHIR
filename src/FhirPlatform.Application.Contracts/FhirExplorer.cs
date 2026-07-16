namespace FhirPlatform.Application.Contracts;

public sealed record FhirExplorerRequest(string ResourceType, string Interaction, string? Id, IReadOnlyDictionary<string, string?> Parameters, string Format = "json");
public sealed record FhirExplorerResponse(int StatusCode, string ResourceType, string Interaction, string ContentType, string Body, IReadOnlyDictionary<string, string> Headers);
