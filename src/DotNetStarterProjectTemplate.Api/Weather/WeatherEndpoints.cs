namespace DotNetStarterProjectTemplate.Api.Weather;

internal static class WeatherEndpoints
{
    internal static void MapWeatherEndpoints(this WebApplication app)
    {
        var adminGroup = app.MapGroup("/api/weather");

        adminGroup.MapGet("/forecast", GetWeatherForecast)
            .WithName(nameof(GetWeatherForecast))
            .WithSummary("Returns Weather Forecast")
            .WithOpenApi();
    }

    private static IResult GetWeatherForecast()
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();

        return Results.Ok(forecast);
    }
}

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
