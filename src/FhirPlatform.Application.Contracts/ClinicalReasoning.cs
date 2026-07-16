namespace FhirPlatform.Application.Contracts;

public sealed record RuleConfigurationDto(Guid Id, string RuleId, string Version, string DisplayName, string ConfigurationJson, bool Active);
public sealed record UpsertRuleConfigurationRequest(string RuleId, string Version, string DisplayName, string ConfigurationJson, bool Active = true);
public sealed record ExecuteRuleRequest(string PatientReference, string RuleId, string? RuleVersion = null);
public sealed record RuleExecutionResultDto(Guid ExecutionId, string PatientReference, string RuleId, string RuleVersion, string Result, IReadOnlyList<string> InputResourceIds, IReadOnlyList<string> OutputResourceIds, DateTimeOffset ExecutedAt);
