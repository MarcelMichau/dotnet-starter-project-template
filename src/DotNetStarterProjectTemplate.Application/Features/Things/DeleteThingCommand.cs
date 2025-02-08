using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace DotNetStarterProjectTemplate.Application.Features.Things;

public sealed record DeleteThingCommand
{
    public required long Id { get; init; }
}

public sealed class DeleteThingCommandHandler(AppDbContext context)
{
    public async Task<Result> Handle(DeleteThingCommand command)
    {
        var thing = await context.Things.FirstOrDefaultAsync(t => t.Id == command.Id);

        if (thing == null)
        {
            return Result.Failure("Thing not found");
        }

        context.Things.Remove(thing);
        await context.SaveChangesAsync();

        return Result.Success();
    }
}
