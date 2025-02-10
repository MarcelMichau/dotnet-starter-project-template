using CSharpFunctionalExtensions;
using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;
using DotNetStarterProjectTemplate.Application.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace DotNetStarterProjectTemplate.Application.Features.Things;

public sealed record GetThingsQuery;

public sealed class GetThingsQueryHandler(AppDbContext context) : IQueryHandler<GetThingsQuery, List<ThingModel>>
{
    public async Task<Result<List<ThingModel>>> Handle(GetThingsQuery query, CancellationToken cancellationToken)
    {
        var things = await context.Things.AsNoTracking().Select(thing => thing.MapToModel()).ToListAsync(cancellationToken);
        return things;
    }
}