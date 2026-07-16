namespace FhirPlatform.Domain;

/// <summary>Validates FHIR Task status transitions used by workflow screens.</summary>
public sealed class TaskStatusTransitionPolicy
{
    private static readonly IReadOnlyDictionary<string, string[]> Allowed = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
    {
        ["draft"] = ["requested", "cancelled"],
        ["requested"] = ["accepted", "rejected", "cancelled"],
        ["accepted"] = ["in-progress", "cancelled"],
        ["in-progress"] = ["completed", "failed", "on-hold", "cancelled"],
        ["on-hold"] = ["in-progress", "cancelled"],
    };

    /// <summary>Returns true when a FHIR Task can move from the current status to the requested status.</summary>
    public bool CanTransition(string currentStatus, string requestedStatus) =>
        Allowed.TryGetValue(currentStatus, out var targets) && targets.Contains(requestedStatus, StringComparer.OrdinalIgnoreCase);
}
