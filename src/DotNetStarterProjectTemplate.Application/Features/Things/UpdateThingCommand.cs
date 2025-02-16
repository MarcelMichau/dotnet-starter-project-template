﻿using CSharpFunctionalExtensions;
using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;
using DotNetStarterProjectTemplate.Application.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace DotNetStarterProjectTemplate.Application.Features.Things;

public sealed record UpdateThingCommand
{
    public required long Id { get; init; }
    public required string Name { get; init; }
}

public sealed class UpdateThingCommandHandler(AppDbContext context) : ICommandHandler<UpdateThingCommand, ThingModel>
{
    public async Task<Result<ThingModel>> Handle(UpdateThingCommand command, CancellationToken cancellationToken)
    {
        var thing = await context.Things.FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (thing == null)
        {
            return Result.Failure<ThingModel>("Thing not found");
        }

        thing.Name = command.Name;

        await context.SaveChangesAsync(cancellationToken);

        return thing.MapToModel();
    }
}