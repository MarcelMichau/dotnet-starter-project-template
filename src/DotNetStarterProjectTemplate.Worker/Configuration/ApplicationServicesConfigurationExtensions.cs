using DotNetStarterProjectTemplate.Application;
using DotNetStarterProjectTemplate.Worker.Data;

namespace DotNetStarterProjectTemplate.Worker.Configuration;

internal static class ApplicationServicesConfigurationExtensions
{
    public static IHostApplicationBuilder AddApplicationServicesConfiguration(this IHostApplicationBuilder builder)
    {
        builder.AddInfrastructure();

        // Only migrate database on startup when running in Development environment
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddHostedService<DatabaseMigrationHostedService>();
        }

        builder.Services.AddHostedService<ThingCountTimerHostedService>();

        return builder;
    }
}