using Asp.Versioning;
using Scalar.AspNetCore;

namespace DotNetStarterProjectTemplate.Api.Configuration;

internal static class ApiVersioningConfigurationExtensions
{
    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddApiVersioningConfiguration()
        {
            builder.Services.AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader(),
                        new HeaderApiVersionReader("X-Api-Version"));
                })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                })
                .AddOpenApi(options => options.Document.AddScalarTransformers());

            return builder;
        }
    }
}
