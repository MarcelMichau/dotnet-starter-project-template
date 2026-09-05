using System.ComponentModel.DataAnnotations;

namespace DotNetStarterProjectTemplate.Api.Filters;

/// <summary>
/// Validates endpoint arguments using Data Annotations before the handler is invoked.
/// Returns a 400 Validation Problem response if any argument is invalid.
/// </summary>
public sealed class ValidationEndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var errors = new Dictionary<string, string[]>();

        foreach (var argument in context.Arguments)
        {
            if (argument is null)
                continue;

            var validationContext = new ValidationContext(argument);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(argument, validationContext, validationResults, validateAllProperties: true))
            {
                foreach (var result in validationResults)
                {
                    var key = result.MemberNames.FirstOrDefault() ?? string.Empty;
                    errors[key] = errors.TryGetValue(key, out var existing)
                        ? [.. existing, result.ErrorMessage ?? "Invalid value."]
                        : [result.ErrorMessage ?? "Invalid value."];
                }
            }
        }

        if (errors.Count > 0)
            return TypedResults.ValidationProblem(errors);

        return await next(context);
    }
}
