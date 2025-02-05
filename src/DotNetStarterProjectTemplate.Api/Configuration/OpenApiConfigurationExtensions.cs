using Scalar.AspNetCore;

namespace DotNetStarterProjectTemplate.Api.Configuration;

internal static class OpenApiConfigurationExtensions
{
    public static IHostApplicationBuilder AddOpenApiConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();

        return builder;
    }

    public static WebApplication MapOpenApiConfiguration(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return app;

        app.MapOpenApi();

        // Use the Aspire external proxy address for the API instead of the internal API address for the URL used by Scalar
        // https://github.com/scalar/scalar/discussions/4025
        app.MapScalarApiReference(options => options.Servers = []);

        return app;
    }
}
