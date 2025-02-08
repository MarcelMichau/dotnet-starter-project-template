using DotNetStarterProjectTemplate.Application;

namespace DotNetStarterProjectTemplate.Api.Configuration;

internal static class ApplicationServicesConfigurationExtensions
{
    public static IHostApplicationBuilder AddApplicationServicesConfiguration(this IHostApplicationBuilder builder)
    {
        builder.AddApplication();
        builder.AddInfrastructure();

        return builder;
    }
}
