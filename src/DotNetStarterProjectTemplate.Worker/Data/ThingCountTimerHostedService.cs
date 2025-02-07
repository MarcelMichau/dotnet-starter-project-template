using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DotNetStarterProjectTemplate.Worker.Data;

internal sealed class ThingCountTimerHostedService(ILogger<ThingCountTimerHostedService> logger, IServiceScopeFactory serviceScopeFactory)
    : BackgroundService
{
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IServiceScopeFactory _serviceScopeFactory =
        serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(10));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken))
            try
            {
                _logger.LogInformation("ThingCountTimerHostedService running at: {time}", DateTimeOffset.Now);
                await GetTotalSurveys(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Oh no! Something bad happened.");
            }
    }

    private async Task GetTotalSurveys(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var thingCount = await context.Things.CountAsync(stoppingToken);

        _logger.LogInformation("Current Number of Things in Database: {ThingCount}", thingCount);
    }

    public override void Dispose()
    {
        _timer.Dispose();
        base.Dispose();
    }
}
