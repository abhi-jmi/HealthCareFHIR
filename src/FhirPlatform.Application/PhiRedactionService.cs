using System.Text.RegularExpressions;

namespace FhirPlatform.Application;

public interface IPhiRedactionService
{
    string Redact(string? value);
}

public sealed partial class PhiRedactionService : IPhiRedactionService
{
    public string Redact(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;
        var redacted = EmailRegex().Replace(value, "[REDACTED_EMAIL]");
        redacted = SsnRegex().Replace(redacted, "[REDACTED_SSN]");
        redacted = MrnRegex().Replace(redacted, "MRN=[REDACTED]");
        redacted = PhoneRegex().Replace(redacted, "[REDACTED_PHONE]");
        return redacted;
    }

    [GeneratedRegex(@"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}", RegexOptions.IgnoreCase | RegexOptions.Compiled)] private static partial Regex EmailRegex();
    [GeneratedRegex(@"\b\d{3}-\d{2}-\d{4}\b", RegexOptions.Compiled)] private static partial Regex SsnRegex();
    [GeneratedRegex(@"MRN\s*=\s*[^\s,;]+", RegexOptions.IgnoreCase | RegexOptions.Compiled)] private static partial Regex MrnRegex();
    [GeneratedRegex(@"\b(?:\+?1[-.\s]?)?\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}\b", RegexOptions.Compiled)] private static partial Regex PhoneRegex();
}
