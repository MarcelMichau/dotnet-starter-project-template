using CSharpFunctionalExtensions;
using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DotNetStarterProjectTemplate.Application.Features.Things;

public sealed record GetThingsQuery;

public sealed class GetThingsQueryHandler(AppDbContext context)
{
    public async Task<Result<List<ThingModel>>> Handle(GetThingsQuery query)
    {
        var things = await context.Things.AsNoTracking().Select(thing => thing.MapToModel()).ToListAsync();
        return things;
    }
}