using FhirPlatform.Api;
using FhirPlatform.Application;
using FhirPlatform.Domain.Shared;
using FhirPlatform.FhirClient;
using FhirPlatform.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration).WriteTo.Console());
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = builder.Configuration.GetValue<long?>("Security:MaxRequestBodyBytes") ?? 10_485_760);
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous", _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = builder.Configuration.GetValue("Security:RateLimit:PermitLimit", 120),
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0
        }));
});
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("FhirPlatform.Api"))
    .WithTracing(tracing => tracing.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddOtlpExporter())
    .WithMetrics(metrics => metrics.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddRuntimeInstrumentation().AddOtlpExporter());
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationSql")));
builder.Services.AddOptions<FhirClientOptions>().Bind(builder.Configuration.GetSection(FhirClientOptions.SectionName)).ValidateDataAnnotations().ValidateOnStart();
builder.Services.AddHttpClient<IFhirResourceClient, MicrosoftFhirResourceClient>((sp, client) => { var options = sp.GetRequiredService<IOptions<FhirClientOptions>>().Value; client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/"); client.Timeout = options.Timeout; });
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IConformanceService, ConformanceService>();
builder.Services.AddScoped<IConformanceCatalogService, ConformanceCatalogService>();
builder.Services.AddScoped<ITerminologyService, TerminologyService>();
builder.Services.AddScoped<IFhirExplorerService, FhirExplorerService>();
builder.Services.AddScoped<IAuditEventService, AuditEventService>();
builder.Services.AddScoped<IResourceValidationService, ResourceValidationService>();
builder.Services.AddSingleton<IPhiRedactionService, PhiRedactionService>();
builder.Services.AddScoped<IExtensionRegistryService, ExtensionRegistryService>();
builder.Services.AddScoped<IExtensionRegistryReader>(sp => sp.GetRequiredService<IExtensionRegistryService>());
builder.Services.AddScoped<IResourceManagementService, ResourceManagementService>();
builder.Services.AddScoped<IFhirBundleIngestionService, FhirBundleIngestionService>();
builder.Services.AddScoped<IClinicalDocumentExtractionService, DeterministicClinicalDocumentExtractionService>();
builder.Services.AddScoped<IClinicalReasoningService, ClinicalReasoningService>();
builder.Services.AddSingleton<IFhirLevelCatalogService, FhirLevelCatalogService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => { builder.Configuration.GetSection("Authentication:JwtBearer").Bind(options); options.RequireHttpsMetadata = builder.Configuration.GetValue("Authentication:RequireHttpsMetadata", false); });
builder.Services.AddAuthorization(options =>
{
    foreach (var permission in Permissions.All)
    {
        options.AddPolicy(permission, policy => policy.RequireAssertion(context =>
            context.User.HasClaim("permission", permission) ||
            context.User.Claims.Where(c => c.Type.EndsWith("/claims/role", StringComparison.OrdinalIgnoreCase) || c.Type == "role" || c.Type == "roles")
                .Any(role => RolePermissionMap.RoleHasPermission(role.Value, permission))));
    }
});
builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("ApplicationSql") ?? string.Empty, name: "application-sql").AddUrlGroup(new Uri(builder.Configuration["Fhir:BaseUrl"] + "/metadata"), name: "fhir-server");
var app = builder.Build();
app.UseExceptionHandler();
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseRateLimiter();
app.UseHttpsRedirection();
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("CorrelationId", httpContext.TraceIdentifier);
        diagnosticContext.Set("RequestPath", httpContext.Request.Path.Value);
        diagnosticContext.Set("RequestMethod", httpContext.Request.Method);
    };
});
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health"); app.MapHealthChecks("/health/live"); app.MapHealthChecks("/health/ready");
app.MapControllers();
app.Run();
public partial class Program { }
