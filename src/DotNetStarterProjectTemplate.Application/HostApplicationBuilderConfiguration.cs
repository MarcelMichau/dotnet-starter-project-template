using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DotNetStarterProjectTemplate.Application.Features.Things;

namespace DotNetStarterProjectTemplate.Application;

public static class HostApplicationBuilderConfiguration
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<GetThingsQueryHandler>();
        builder.Services.AddScoped<GetThingByIdQueryHandler>();
        builder.Services.AddScoped<DeleteThingCommandHandler>();
        builder.Services.AddScoped<CreateThingCommandHandler>();
        builder.Services.AddScoped<UpdateThingCommandHandler>();

        return builder;
    }

    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton(TimeProvider.System);

        builder.AddDatabaseConfiguration(builder.Configuration);

        return builder;
    }
}