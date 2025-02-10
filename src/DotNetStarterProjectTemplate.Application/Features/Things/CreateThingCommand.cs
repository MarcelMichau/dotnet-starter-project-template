using CSharpFunctionalExtensions;
using DotNetStarterProjectTemplate.Application.Domain.Things;
using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;
using DotNetStarterProjectTemplate.Application.Shared.Utils;

namespace DotNetStarterProjectTemplate.Application.Features.Things;

public sealed record CreateThingCommand
{
    public required string Name { get; init; }
}

public sealed class CreateThingCommandHandler(AppDbContext context) : ICommandHandler<CreateThingCommand, ThingModel>
{
    public async Task<Result<ThingModel>> Handle(CreateThingCommand command, CancellationToken cancellationToken)
    {
        var thing = new Thing
        {
            Name = command.Name
        };

        context.Things.Add(thing);
        await context.SaveChangesAsync(cancellationToken);

        return thing.MapToModel();
    }
}