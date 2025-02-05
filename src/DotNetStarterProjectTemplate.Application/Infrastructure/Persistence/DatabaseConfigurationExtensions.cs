using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;

internal static class DatabaseConfigurationExtensions
{
    public static IHostApplicationBuilder AddDatabaseConfiguration(this IHostApplicationBuilder builder,
        IConfiguration configuration)
    {
        const string connectionName = "database";

        builder.AddSqlServerDbContext<AppDbContext>(connectionName);

        return builder;
    }
}