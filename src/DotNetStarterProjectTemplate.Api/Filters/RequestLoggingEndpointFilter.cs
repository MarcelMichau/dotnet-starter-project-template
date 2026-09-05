using System.Diagnostics;

namespace DotNetStarterProjectTemplate.Api.Filters;

public sealed class RequestLoggingEndpointFilter(ILoggerFactory loggerFactory) : IEndpointFilter
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<RequestLoggingEndpointFilter>();

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var endpointMetadataCollection = context.HttpContext.GetEndpoint()?.Metadata;
        var endpointName = endpointMetadataCollection?.GetMetadata<EndpointNameMetadata>()?.EndpointName;

        _logger.LogInformation("Request to Endpoint: {EndpointName}", endpointName);

        var stopwatch = Stopwatch.StartNew();
        var result = await next(context);
        stopwatch.Stop();

        var statusCode = context.HttpContext.Response.StatusCode;
        _logger.LogInformation("Endpoint {EndpointName} responded with {StatusCode} in {ElapsedMs}ms",
            endpointName, statusCode, stopwatch.ElapsedMilliseconds);

        return result;
    }
}
