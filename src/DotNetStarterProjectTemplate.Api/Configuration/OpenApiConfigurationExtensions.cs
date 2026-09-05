using DotNetStarterProjectTemplate.Application.Shared;
using Scalar.AspNetCore;

namespace DotNetStarterProjectTemplate.Api.Configuration;

internal static class OpenApiConfigurationExtensions
{
    extension(WebApplication app)
    {
        public WebApplication MapOpenApiConfiguration()
        {
            app.MapOpenApi().WithDocumentPerVersion();

            app.MapScalarApiReference(options =>
            {
                options.Title = $"{Constants.AppFriendlyName} - OpenAPI";

                // Because light attracts bugs :)
                options.DarkMode = true;
                options.HideDarkModeToggle = true;

                // Use the Aspire external proxy address for the API instead of the internal API address for the URL used by Scalar
                // https://github.com/scalar/scalar/discussions/4025
                options.Servers = [];

                var descriptions = app.DescribeApiVersions();

                for (var i = 0; i < descriptions.Count; i++)
                {
                    var description = descriptions[i];
                    var isDefault = i == descriptions.Count - 1;

                    options.AddDocument(description.GroupName, description.GroupName, isDefault: isDefault);
                }
            });

            return app;
        }
    }
}
