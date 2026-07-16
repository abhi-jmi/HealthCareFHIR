using System.ComponentModel.DataAnnotations;

namespace FhirPlatform.FhirClient;

public sealed class FhirClientOptions
{
    public const string SectionName = "Fhir";
    [Required, Url] public string BaseUrl { get; init; } = "http://fhir-server:8080";
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);
}
