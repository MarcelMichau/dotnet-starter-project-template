using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetStarterProjectTemplate.Api.Things;

internal static class ThingEndpoints
{
    internal static void MapThingEndpoints(this WebApplication app)
    {
        var adminGroup = app.MapGroup("/api/things");

        adminGroup.MapGet("/", GetThings)
            .WithName(nameof(GetThings))
            .WithSummary("Returns Things from the DB")
            .WithOpenApi();
    }

    private static async Task<IResult> GetThings([FromServices] ILoggerFactory loggerFactory,
        [FromServices] AppDbContext context)
    {
        var logger = loggerFactory.CreateLogger(nameof(ThingEndpoints));
        logger.LogInformation("Retrieving things...");

        var things = await context.Things.AsNoTracking().Select(thing => thing.MapToModel()).ToListAsync();

        return Results.Ok(things);
    }
}