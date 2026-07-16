using FhirPlatform.Domain;
using FluentAssertions;

namespace FhirPlatform.Domain.Tests;

public sealed class TaskStatusTransitionPolicyTests
{
    [Fact]
    public void CanTransition_allows_requested_to_accepted() => new TaskStatusTransitionPolicy().CanTransition("requested", "accepted").Should().BeTrue();

    [Fact]
    public void CanTransition_rejects_completed_to_in_progress() => new TaskStatusTransitionPolicy().CanTransition("completed", "in-progress").Should().BeFalse();
}
