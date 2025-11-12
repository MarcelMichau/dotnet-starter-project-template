using System.ComponentModel;
using DotNetStarterProjectTemplate.Api.Filters;
using DotNetStarterProjectTemplate.Application.Features.Things;
using DotNetStarterProjectTemplate.Application.Shared.Utils;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DotNetStarterProjectTemplate.Api.Things;

internal static class ThingEndpoints
{
    extension(WebApplication app)
    {
        internal void MapThingEndpoints()
        {
            var thingGroup = app.MapGroup("/api/things")
                .AddEndpointFilter<RequestLoggingEndpointFilter>();

            thingGroup.MapGet("/", GetThings)
                .WithName(nameof(GetThings))
                .WithSummary("Returns Things from the DB");

            thingGroup.MapGet("/{id:long}", GetThingById)
                .WithName(nameof(GetThingById))
                .WithSummary("Returns a Thing by ID from the DB");

            thingGroup.MapPost("/", CreateThing)
                .WithName(nameof(CreateThing))
                .WithSummary("Creates a new Thing in the DB");

            thingGroup.MapPut("/{id:long}", UpdateThing)
                .WithName(nameof(UpdateThing))
                .WithSummary("Updates an existing Thing in the DB");

            thingGroup.MapDelete("/{id:long}", DeleteThing)
                .WithName(nameof(DeleteThing))
                .WithSummary("Deletes a Thing by ID from the DB");
        }
    }

    private static async Task<Ok<List<ThingModel>>> GetThings(IQueryHandler<GetThingsQuery, List<ThingModel>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetThingsQuery(), cancellationToken);

        return TypedResults.Ok(result.Value);
    }

    private static async Task<Results<NotFound, Ok<ThingModel>>> GetThingById(
        [Description("Primary Key of the Thing")]
        long id, IQueryHandler<GetThingByIdQuery, ThingModel> handler, CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetThingByIdQuery { Id = id }, cancellationToken);

        if (result.IsFailure)
            return TypedResults.NotFound();

        return TypedResults.Ok(result.Value);
    }

    private static async Task<CreatedAtRoute<ThingModel>> CreateThing(CreateThingCommand command,
        ICommandHandler<CreateThingCommand, ThingModel> handler, CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);

        return TypedResults.CreatedAtRoute(result.Value, nameof(GetThingById), new { id = result.Value.Id });
    }

    private static async Task<Results<BadRequest, NotFound, Ok<ThingModel>>> UpdateThing(
        [Description("Primary Key of the Thing")]
        long id,
        UpdateThingCommand command, ICommandHandler<UpdateThingCommand, ThingModel> handler,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return TypedResults.BadRequest();

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return TypedResults.NotFound();

        return TypedResults.Ok(result.Value);
    }

    private static async Task<Results<NotFound, Ok>> DeleteThing([Description("Primary Key of the Thing")] long id,
        ICommandHandler<DeleteThingCommand> handler, CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new DeleteThingCommand { Id = id }, cancellationToken);

        if (result.IsFailure)
            return TypedResults.NotFound();

        return TypedResults.Ok();
    }
}