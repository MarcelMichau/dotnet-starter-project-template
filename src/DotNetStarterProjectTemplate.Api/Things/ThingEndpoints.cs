using System.ComponentModel;
using DotNetStarterProjectTemplate.Api.Filters;
using DotNetStarterProjectTemplate.Application.Features.Things;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DotNetStarterProjectTemplate.Api.Things;

internal static class ThingEndpoints
{
    internal static void MapThingEndpoints(this WebApplication app)
    {
        var adminGroup = app.MapGroup("/api/things")
            .AddEndpointFilter<RequestLoggingEndpointFilter>();

        adminGroup.MapGet("/", GetThings)
            .WithName(nameof(GetThings))
            .WithSummary("Returns Things from the DB");

        adminGroup.MapGet("/{id:long}", GetThingById)
            .WithName(nameof(GetThingById))
            .WithSummary("Returns a Thing by ID from the DB");

        adminGroup.MapPost("/", CreateThing)
            .WithName(nameof(CreateThing))
            .WithSummary("Creates a new Thing in the DB");

        adminGroup.MapPut("/{id:long}", UpdateThing)
            .WithName(nameof(UpdateThing))
            .WithSummary("Updates an existing Thing in the DB");

        adminGroup.MapDelete("/{id:long}", DeleteThing)
            .WithName(nameof(DeleteThing))
            .WithSummary("Deletes a Thing by ID from the DB");
    }

    private static async Task<Ok<List<ThingModel>>> GetThings(GetThingsQueryHandler handler)
    {
        var result = await handler.Handle(new GetThingsQuery());

        return TypedResults.Ok(result.Value);
    }

    private static async Task<Results<NotFound, Ok<ThingModel>>> GetThingById(
        [Description("Primary Key of the Thing")]
        long id, GetThingByIdQueryHandler handler)
    {
        var result = await handler.Handle(new GetThingByIdQuery { Id = id });

        if (result.IsFailure)
            return TypedResults.NotFound();

        return TypedResults.Ok(result.Value);
    }

    private static async Task<CreatedAtRoute<ThingModel>> CreateThing(CreateThingCommand command,
        CreateThingCommandHandler handler)
    {
        var result = await handler.Handle(command);

        return TypedResults.CreatedAtRoute(result.Value, nameof(GetThingById), new { id = result.Value.Id });
    }

    private static async Task<Results<BadRequest, NotFound, Ok<ThingModel>>> UpdateThing(
        [Description("Primary Key of the Thing")]
        long id,
        UpdateThingCommand command, UpdateThingCommandHandler handler)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        var result = await handler.Handle(command);

        if (result.IsFailure)
            return TypedResults.NotFound();

        return TypedResults.Ok(result.Value);
    }

    private static async Task<Results<NotFound, Ok>> DeleteThing([Description("Primary Key of the Thing")] long id,
        DeleteThingCommandHandler handler)
    {
        var result = await handler.Handle(new DeleteThingCommand { Id = id });

        if (result.IsFailure)
            return TypedResults.NotFound();

        return TypedResults.Ok();
    }
}