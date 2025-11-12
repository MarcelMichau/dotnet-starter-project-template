using DotNetStarterProjectTemplate.Application;

namespace DotNetStarterProjectTemplate.Api.Configuration;

internal static class ApplicationServicesConfigurationExtensions
{
    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddApplicationServicesConfiguration()
        {
            builder.AddApplication();
            builder.AddInfrastructure();

            return builder;
        }
    }
}
