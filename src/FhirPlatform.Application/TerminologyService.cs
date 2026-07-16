using FhirPlatform.Application.Contracts;
using FhirPlatform.FhirClient;
using Hl7.Fhir.Model;

namespace FhirPlatform.Application;

public interface ITerminologyService
{
    Task<IReadOnlyList<TerminologyResourceSummaryDto>> SearchCodeSystemsAsync(TerminologySearchRequest request, CancellationToken cancellationToken);
    Task<IReadOnlyList<TerminologyResourceSummaryDto>> SearchValueSetsAsync(TerminologySearchRequest request, CancellationToken cancellationToken);
    Task<IReadOnlyList<TerminologyResourceSummaryDto>> SearchConceptMapsAsync(TerminologySearchRequest request, CancellationToken cancellationToken);
    Task<IReadOnlyList<TerminologyCodingDto>> ExpandValueSetAsync(ValueSetExpandRequest request, CancellationToken cancellationToken);
    Task<CodeValidationResponse> ValidateCodeAsync(CodeValidationRequest request, CancellationToken cancellationToken);
    Task<ConceptTranslateResponse> TranslateAsync(ConceptTranslateRequest request, CancellationToken cancellationToken);
}

public sealed class TerminologyService(IFhirResourceClient fhirClient) : ITerminologyService
{
    public async Task<IReadOnlyList<TerminologyResourceSummaryDto>> SearchCodeSystemsAsync(TerminologySearchRequest request, CancellationToken cancellationToken)
    {
        var bundle = await fhirClient.SearchAsync<CodeSystem>(SearchParameters(request), cancellationToken);
        return bundle.Entry.Select(e => e.Resource).OfType<CodeSystem>().Select(cs => new TerminologyResourceSummaryDto("CodeSystem", cs.Id, cs.Url, cs.Version, cs.Name, cs.Title, cs.Status?.ToString())).ToArray();
    }

    public async Task<IReadOnlyList<TerminologyResourceSummaryDto>> SearchValueSetsAsync(TerminologySearchRequest request, CancellationToken cancellationToken)
    {
        var bundle = await fhirClient.SearchAsync<ValueSet>(SearchParameters(request), cancellationToken);
        return bundle.Entry.Select(e => e.Resource).OfType<ValueSet>().Select(vs => new TerminologyResourceSummaryDto("ValueSet", vs.Id, vs.Url, vs.Version, vs.Name, vs.Title, vs.Status?.ToString())).ToArray();
    }

    public async Task<IReadOnlyList<TerminologyResourceSummaryDto>> SearchConceptMapsAsync(TerminologySearchRequest request, CancellationToken cancellationToken)
    {
        var bundle = await fhirClient.SearchAsync<ConceptMap>(SearchParameters(request), cancellationToken);
        return bundle.Entry.Select(e => e.Resource).OfType<ConceptMap>().Select(cm => new TerminologyResourceSummaryDto("ConceptMap", cm.Id, cm.Url, cm.Version, cm.Name, cm.Title, cm.Status?.ToString())).ToArray();
    }

    public async Task<IReadOnlyList<TerminologyCodingDto>> ExpandValueSetAsync(ValueSetExpandRequest request, CancellationToken cancellationToken)
    {
        var parameters = new Parameters();
        parameters.Add("url", new FhirUri(request.ValueSetUrl));
        parameters.Add("count", new Integer(request.Count));
        if (!string.IsNullOrWhiteSpace(request.Filter)) parameters.Add("filter", new FhirString(request.Filter));
        var result = await fhirClient.ExecuteOperationAsync("ValueSet/$expand", parameters, cancellationToken);
        var valueSet = result as ValueSet;
        return valueSet?.Expansion?.Contains.Select(c => new TerminologyCodingDto(c.System, c.Code, c.Display)).ToArray() ?? Array.Empty<TerminologyCodingDto>();
    }

    public async Task<CodeValidationResponse> ValidateCodeAsync(CodeValidationRequest request, CancellationToken cancellationToken)
    {
        var parameters = new Parameters();
        parameters.Add("url", new FhirUri(request.ValueSetUrl));
        parameters.Add("system", new FhirUri(request.System));
        parameters.Add("code", new Code(request.Code));
        if (!string.IsNullOrWhiteSpace(request.Display)) parameters.Add("display", new FhirString(request.Display));
        var result = await fhirClient.ExecuteOperationAsync("ValueSet/$validate-code", parameters, cancellationToken) as Parameters;
        return new CodeValidationResponse(GetBool(result, "result"), GetString(result, "message"), GetString(result, "display"));
    }

    public async Task<ConceptTranslateResponse> TranslateAsync(ConceptTranslateRequest request, CancellationToken cancellationToken)
    {
        var parameters = new Parameters();
        parameters.Add("url", new FhirUri(request.ConceptMapUrl));
        parameters.Add("system", new FhirUri(request.System));
        parameters.Add("code", new Code(request.Code));
        if (!string.IsNullOrWhiteSpace(request.TargetSystem)) parameters.Add("targetsystem", new FhirUri(request.TargetSystem));
        var result = await fhirClient.ExecuteOperationAsync("ConceptMap/$translate", parameters, cancellationToken) as Parameters;
        var matches = result?.Parameter.Where(p => p.Name == "match").SelectMany(p => p.Part).Where(p => p.Name == "concept" && p.Value is Coding).Select(p => (Coding)p.Value!).Select(c => new TerminologyCodingDto(c.System, c.Code, c.Display)).ToArray() ?? Array.Empty<TerminologyCodingDto>();
        return new ConceptTranslateResponse(GetBool(result, "result"), GetString(result, "message"), matches);
    }

    private static Dictionary<string, string?> SearchParameters(TerminologySearchRequest request) => new() { ["_count"] = request.Count.ToString(), ["name:contains"] = request.Filter, ["title:contains"] = request.Filter };
    private static bool GetBool(Parameters? parameters, string name) => parameters?.Parameter.FirstOrDefault(p => p.Name == name)?.Value is FhirBoolean value && value.Value == true;
    private static string? GetString(Parameters? parameters, string name) => parameters?.Parameter.FirstOrDefault(p => p.Name == name)?.Value switch { FhirString s => s.Value, FhirUri u => u.Value, Code c => c.Value, FhirBoolean b => b.Value?.ToString(), _ => null };
}
