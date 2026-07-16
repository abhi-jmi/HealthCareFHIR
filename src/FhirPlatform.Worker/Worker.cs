namespace FhirPlatform.Worker;
public sealed class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) { logger.LogInformation("FHIR platform worker heartbeat"); await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); }
    }
}
