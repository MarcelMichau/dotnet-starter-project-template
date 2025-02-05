using DotNetStarterProjectTemplate.Application.Domain.Things;
using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DotNetStarterProjectTemplate.Api.Data;

internal sealed class DatabaseMigrationHostedService(
    IServiceProvider serviceProvider,
    ILogger<DatabaseMigrationHostedService> logger)
    : IHostedService
{
    private readonly ILogger<DatabaseMigrationHostedService> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IServiceProvider _serviceProvider =
        serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var scope = _serviceProvider.CreateScope();

        await using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (context.Database
            .IsSqlServer()) // Do not migrate database when running integration tests with in-memory database
        {
            _logger.LogInformation("Creating/Migrating Database...");

            try
            {
                await context.Database.MigrateAsync(cancellationToken);

                if (!context.Things.Any())
                {
                    context.Things.AddRange(
                        new Thing { Name = "Thing 1" },
                        new Thing { Name = "Thing 2" },
                        new Thing { Name = "Thing 3" });

                    await context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while migrating the database");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
