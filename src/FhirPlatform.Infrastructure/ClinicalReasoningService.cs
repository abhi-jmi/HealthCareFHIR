using System.Text.Json;
using FhirPlatform.Application;
using FhirPlatform.Application.Contracts;
using FhirPlatform.FhirClient;
using Hl7.Fhir.Model;
using Microsoft.EntityFrameworkCore;

namespace FhirPlatform.Infrastructure;

public sealed class ClinicalReasoningService(AppDbContext db, IFhirResourceClient fhirClient) : IClinicalReasoningService
{
    public async Task<IReadOnlyList<RuleConfigurationDto>> ListRulesAsync(CancellationToken cancellationToken) =>
        await db.RuleConfigurations.OrderBy(x => x.RuleId).ThenByDescending(x => x.Version).Select(x => new RuleConfigurationDto(x.Id, x.RuleId, x.Version, x.DisplayName, x.ConfigurationJson, x.Active)).ToArrayAsync(cancellationToken);

    public async Task<RuleConfigurationDto> UpsertRuleAsync(UpsertRuleConfigurationRequest request, CancellationToken cancellationToken)
    {
        using var _ = JsonDocument.Parse(request.ConfigurationJson);
        var entity = await db.RuleConfigurations.SingleOrDefaultAsync(x => x.RuleId == request.RuleId && x.Version == request.Version, cancellationToken);
        if (entity is null) { entity = new RuleConfiguration { Id = Guid.NewGuid(), RuleId = request.RuleId, Version = request.Version, DisplayName = request.DisplayName, ConfigurationJson = request.ConfigurationJson, Active = request.Active }; db.RuleConfigurations.Add(entity); }
        else { entity.DisplayName = request.DisplayName; entity.ConfigurationJson = request.ConfigurationJson; entity.Active = request.Active; }
        await db.SaveChangesAsync(cancellationToken);
        return new RuleConfigurationDto(entity.Id, entity.RuleId, entity.Version, entity.DisplayName, entity.ConfigurationJson, entity.Active);
    }

    public async Task<RuleExecutionResultDto> ExecuteAsync(ExecuteRuleRequest request, string initiatedBy, CancellationToken cancellationToken)
    {
        var rule = await db.RuleConfigurations.Where(x => x.RuleId == request.RuleId && x.Active && (request.RuleVersion == null || x.Version == request.RuleVersion)).OrderByDescending(x => x.Version).FirstAsync(cancellationToken);
        using var json = JsonDocument.Parse(rule.ConfigurationJson);
        var reviewMessage = json.RootElement.TryGetProperty("reviewMessage", out var message) ? message.GetString() : "Configured clinical review reminder.";
        var observations = await fhirClient.SearchAsync<Observation>(new Dictionary<string, string?> { ["subject"] = request.PatientReference, ["code"] = json.RootElement.TryGetProperty("observationCode", out var code) ? code.GetString() : null, ["_sort"] = "-date", ["_count"] = "1" }, cancellationToken);
        var inputs = observations.Entry.Select(e => e.Resource).Where(r => r is not null).Select(r => $"{r.TypeName}/{r.Id}").ToArray();
        var guidance = new GuidanceResponse { Status = GuidanceResponse.GuidanceResponseStatus.Success, Subject = new ResourceReference(request.PatientReference), Module = new Canonical($"PlanDefinition/{rule.RuleId}|{rule.Version}"), Note = [new Annotation { Text = reviewMessage }] };
        var created = await fhirClient.CreateAsync(guidance, cancellationToken);
        var audit = new RuleExecutionAudit { Id = Guid.NewGuid(), PatientReference = request.PatientReference, RuleId = rule.RuleId, RuleVersion = rule.Version, InputResourceIds = string.Join(',', inputs), OutputResourceIds = $"GuidanceResponse/{created.Id}", Result = "Success", InitiatedBy = initiatedBy };
        db.RuleExecutionAudits.Add(audit); await db.SaveChangesAsync(cancellationToken);
        return new RuleExecutionResultDto(audit.Id, audit.PatientReference, audit.RuleId, audit.RuleVersion, audit.Result, inputs, [audit.OutputResourceIds], audit.ExecutedAt);
    }
}
