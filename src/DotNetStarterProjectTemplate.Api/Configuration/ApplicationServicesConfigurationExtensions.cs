using DotNetStarterProjectTemplate.Api.Data;
using DotNetStarterProjectTemplate.Application;

namespace DotNetStarterProjectTemplate.Api.Configuration;

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

        return builder;
    }
}
