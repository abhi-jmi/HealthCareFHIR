namespace FhirPlatform.Web;

/// <summary>Adds baseline browser security headers to web responses.</summary>
/// <param name="next">The next middleware in the request pipeline.</param>
public sealed class SecurityHeadersMiddleware(RequestDelegate next)
{
    /// <summary>Adds security response headers before passing the request to the next middleware.</summary>
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            var headers = context.Response.Headers;
            headers.TryAdd("X-Content-Type-Options", "nosniff");
            headers.TryAdd("X-Frame-Options", "DENY");
            headers.TryAdd("Referrer-Policy", "no-referrer");
            headers.TryAdd("Permissions-Policy", "camera=(), microphone=(), geolocation=()");
            headers.TryAdd("Content-Security-Policy", "default-src 'self'; connect-src 'self' http://application-api:8080 https:; style-src 'self' 'unsafe-inline'; script-src 'self' 'unsafe-inline'; frame-ancestors 'none'; object-src 'none'");
            if (!context.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment()) headers.TryAdd("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
            return Task.CompletedTask;
        });
        await next(context);
    }
}
