using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using DotNetStarterProjectTemplate.Application.Shared.Utils;

namespace DotNetStarterProjectTemplate.Application.Features.Things;

public sealed record DeleteThingCommand
{
    public required long Id { get; init; }
}

public sealed class DeleteThingCommandHandler(AppDbContext context) : ICommandHandler<DeleteThingCommand>
{
    public async Task<Result> Handle(DeleteThingCommand command, CancellationToken cancellationToken)
    {
        var thing = await context.Things.FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (thing == null)
        {
            return Result.Failure("Thing not found");
        }

        context.Things.Remove(thing);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
