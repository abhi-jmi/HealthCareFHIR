using FhirPlatform.Application.Contracts;

namespace FhirPlatform.Application;

public interface IClinicalReasoningService
{
    Task<IReadOnlyList<RuleConfigurationDto>> ListRulesAsync(CancellationToken cancellationToken);
    Task<RuleConfigurationDto> UpsertRuleAsync(UpsertRuleConfigurationRequest request, CancellationToken cancellationToken);
    Task<RuleExecutionResultDto> ExecuteAsync(ExecuteRuleRequest request, string initiatedBy, CancellationToken cancellationToken);
}
