using FhirPlatform.Api.Controllers;
using FhirPlatform.Domain.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FhirPlatform.Api.IntegrationTests;

public sealed class ValidationEndpointTests
{
    [Fact]
    public void Validation_endpoint_is_routed_and_policy_protected()
    {
        typeof(ValidationController).GetCustomAttributes(typeof(RouteAttribute), inherit: false)
            .Cast<RouteAttribute>().Single().Template.Should().Be("api/fhir/validation");

        var authorize = typeof(ValidationController).GetMethod(nameof(ValidationController.Validate))!
            .GetCustomAttributes(typeof(AuthorizeAttribute), inherit: false)
            .Cast<AuthorizeAttribute>().Single();

        authorize.Policy.Should().Be(Permissions.ConformanceManage);
    }
}
