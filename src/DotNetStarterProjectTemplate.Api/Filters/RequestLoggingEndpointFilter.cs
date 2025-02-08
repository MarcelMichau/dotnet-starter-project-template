
namespace DotNetStarterProjectTemplate.Api.Filters;

public class RequestLoggingEndpointFilter(ILoggerFactory loggerFactory) : IEndpointFilter
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<RequestLoggingEndpointFilter>();

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var endpointMetadataCollection = context.HttpContext.GetEndpoint()?.Metadata;
        var endpointName = endpointMetadataCollection?.GetMetadata<EndpointNameMetadata>()?.EndpointName;

        _logger.LogInformation("Request to Endpoint: {EndpointName}", endpointName);
        var result = await next(context);
        return result;
    }
}
