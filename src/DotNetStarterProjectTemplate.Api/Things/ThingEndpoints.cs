using DotNetStarterProjectTemplate.Application.Domain.Things;
using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
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

        adminGroup.MapGet("/{id:long}", GetThingById)
            .WithName(nameof(GetThingById))
            .WithSummary("Returns a Thing by ID from the DB")
            .WithOpenApi();

        adminGroup.MapPost("/", CreateThing)
            .WithName(nameof(CreateThing))
            .WithSummary("Creates a new Thing in the DB")
            .WithOpenApi();

        adminGroup.MapPut("/{id:long}", UpdateThing)
            .WithName(nameof(UpdateThing))
            .WithSummary("Updates an existing Thing in the DB")
            .WithOpenApi();

        adminGroup.MapDelete("/{id:long}", DeleteThing)
            .WithName(nameof(DeleteThing))
            .WithSummary("Deletes a Thing by ID from the DB")
            .WithOpenApi();
    }

    private static async Task<Ok<List<ThingModel>>> GetThings(ILoggerFactory loggerFactory, AppDbContext context)
    {
        var logger = loggerFactory.CreateLogger(nameof(ThingEndpoints));
        logger.LogInformation("Retrieving things...");

        var things = await context.Things.AsNoTracking().Select(thing => thing.MapToModel()).ToListAsync();

        return TypedResults.Ok(things);
    }

    private static async Task<Results<NotFound, Ok<ThingModel>>> GetThingById(long id, ILoggerFactory loggerFactory, AppDbContext context)
    {
        var logger = loggerFactory.CreateLogger(nameof(ThingEndpoints));
        logger.LogInformation("Retrieving thing with ID {id}...", id);

        var thing = await context.Things.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (thing == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(thing.MapToModel());
    }

    private static async Task<CreatedAtRoute<ThingModel>> CreateThing(ThingModel thing, ILoggerFactory loggerFactory, AppDbContext context)
    {
        var logger = loggerFactory.CreateLogger(nameof(ThingEndpoints));
        logger.LogInformation("Creating a new thing...");

        var newThing = new Thing
        {
            Name = thing.Name
        };

        context.Things.Add(newThing);
        await context.SaveChangesAsync();

        return TypedResults.CreatedAtRoute(newThing.MapToModel(), nameof(GetThingById), new { id = newThing.Id });
    }

    private static async Task<Results<BadRequest, NotFound, Ok<ThingModel>>> UpdateThing(long id,
        ThingModel updatedThing, ILoggerFactory loggerFactory, AppDbContext context)
    {
        if (id != updatedThing.Id)
        {
            return TypedResults.BadRequest();
        }

        var logger = loggerFactory.CreateLogger(nameof(ThingEndpoints));
        logger.LogInformation("Updating thing with ID {id}...", id);

        var existingThing = await context.Things.FirstOrDefaultAsync(t => t.Id == id);
        if (existingThing == null)
        {
            return TypedResults.NotFound();
        }

        existingThing.Name = updatedThing.Name;
        await context.SaveChangesAsync();

        return TypedResults.Ok(existingThing.MapToModel());
    }

    private static async Task<Results<NotFound, Ok>> DeleteThing(long id, ILoggerFactory loggerFactory,
        AppDbContext context)
    {
        var logger = loggerFactory.CreateLogger(nameof(ThingEndpoints));
        logger.LogInformation("Deleting thing with ID {id}...", id);

        var thing = await context.Things.FirstOrDefaultAsync(t => t.Id == id);
        if (thing == null)
        {
            return TypedResults.NotFound();
        }

        context.Things.Remove(thing);
        await context.SaveChangesAsync();

        return TypedResults.Ok();
    }
}