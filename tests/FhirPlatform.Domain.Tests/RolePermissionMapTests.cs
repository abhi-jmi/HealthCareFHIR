using FhirPlatform.Domain.Shared;
using FluentAssertions;

namespace FhirPlatform.Domain.Tests;

public sealed class RolePermissionMapTests
{
    [Fact]
    public void Clinician_has_patient_read_but_not_audit_read()
    {
        RolePermissionMap.RoleHasPermission(PlatformRoles.Clinician, Permissions.PatientRead).Should().BeTrue();
        RolePermissionMap.RoleHasPermission(PlatformRoles.Clinician, Permissions.AuditRead).Should().BeFalse();
    }
}
