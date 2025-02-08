using CSharpFunctionalExtensions;
using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DotNetStarterProjectTemplate.Application.Features.Things;

public sealed record GetThingByIdQuery
{
    public required long Id { get; init; }
}

public sealed class GetThingByIdQueryHandler(AppDbContext context)
{
    public async Task<Result<ThingModel>> Handle(GetThingByIdQuery query)
    {
        var thing = await context.Things.AsNoTracking().FirstOrDefaultAsync(t => t.Id == query.Id);
        return thing?.MapToModel() ?? Result.Failure<ThingModel>("Thing Not Found");
    }
}