using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DotNetStarterProjectTemplate.Application.Features.Things;
using DotNetStarterProjectTemplate.Application.Shared.Utils;

namespace DotNetStarterProjectTemplate.Application;

public static class HostApplicationBuilderConfiguration
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IQueryHandler<GetThingsQuery, List<ThingModel>>, GetThingsQueryHandler>();
        builder.Services.AddScoped<IQueryHandler<GetThingByIdQuery, ThingModel>, GetThingByIdQueryHandler>();
        builder.Services.AddScoped<ICommandHandler<DeleteThingCommand>, DeleteThingCommandHandler>();
        builder.Services.AddScoped<ICommandHandler<CreateThingCommand, ThingModel>, CreateThingCommandHandler>();
        builder.Services.AddScoped<ICommandHandler<UpdateThingCommand, ThingModel>, UpdateThingCommandHandler>();

        return builder;
    }

    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton(TimeProvider.System);

        builder.AddDatabaseConfiguration(builder.Configuration);

        return builder;
    }
}