namespace FhirPlatform.Api;

public sealed class SecurityHeadersMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            var headers = context.Response.Headers;
            headers.TryAdd("X-Content-Type-Options", "nosniff");
            headers.TryAdd("X-Frame-Options", "DENY");
            headers.TryAdd("Referrer-Policy", "no-referrer");
            headers.TryAdd("Permissions-Policy", "camera=(), microphone=(), geolocation=()");
            headers.TryAdd("Content-Security-Policy", "default-src 'self'; frame-ancestors 'none'; object-src 'none'");
            if (!context.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment())
            {
                headers.TryAdd("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
            }
            return Task.CompletedTask;
        });
        await next(context);
    }
}
