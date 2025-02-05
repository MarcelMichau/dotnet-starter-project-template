using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DotNetStarterProjectTemplate.Application;

public static class HostApplicationBuilderConfiguration
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton(TimeProvider.System);

        builder.AddDatabaseConfiguration(builder.Configuration);

        return builder;
    }
}