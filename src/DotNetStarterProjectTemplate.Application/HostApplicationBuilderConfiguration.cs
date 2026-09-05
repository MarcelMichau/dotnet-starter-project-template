using System.Reflection;
using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;
using DotNetStarterProjectTemplate.Application.Shared.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DotNetStarterProjectTemplate.Application;

public static class HostApplicationBuilderConfiguration
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Auto-register all ICommandHandler<TRequest>, ICommandHandler<TRequest, TResponse>
        // and IQueryHandler<TRequest, TResponse> implementations found in this assembly.
        foreach (var type in assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract))
        {
            foreach (var iface in type.GetInterfaces())
            {
                if (!iface.IsGenericType) continue;

                var genericDef = iface.GetGenericTypeDefinition();
                if (genericDef == typeof(ICommandHandler<>) ||
                    genericDef == typeof(ICommandHandler<,>) ||
                    genericDef == typeof(IQueryHandler<,>))
                {
                    builder.Services.AddScoped(iface, type);
                }
            }
        }

        return builder;
    }

    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton(TimeProvider.System);

        builder.AddDatabaseConfiguration();

        return builder;
    }
}