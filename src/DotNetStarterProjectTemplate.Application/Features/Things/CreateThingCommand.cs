using CSharpFunctionalExtensions;
using DotNetStarterProjectTemplate.Application.Domain.Things;
using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;

namespace DotNetStarterProjectTemplate.Application.Features.Things;

public sealed record CreateThingCommand
{
    public required string Name { get; init; }
}

public sealed class CreateThingCommandHandler(AppDbContext context)
{
    public async Task<Result<ThingModel>> Handle(CreateThingCommand command)
    {
        var thing = new Thing
        {
            Name = command.Name
        };

        context.Things.Add(thing);
        await context.SaveChangesAsync();

        return thing.MapToModel();
    }
}