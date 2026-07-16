using FluentAssertions;

namespace FhirPlatform.Web.Tests;

public sealed class ValidationPageTests
{
    [Fact]
    public void Validation_page_calls_api_and_supports_downloads()
    {
        var root = FindRepositoryRoot();
        var markup = File.ReadAllText(Path.Combine(root, "src", "FhirPlatform.Web", "Pages", "Validation.razor"));
        markup.Should().Contain("PostAsJsonAsync(\"api/fhir/validation\"");
        markup.Should().Contain("Download JSON");
        markup.Should().Contain("Download XML");
        markup.Should().Contain("OperationOutcomePanel");
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "FhirPlatform.sln")))
        {
            directory = directory.Parent;
        }
        return directory?.FullName ?? throw new InvalidOperationException("Repository root was not found.");
    }
}
