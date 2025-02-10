using CSharpFunctionalExtensions;
using DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;
using DotNetStarterProjectTemplate.Application.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace DotNetStarterProjectTemplate.Application.Features.Things;

public sealed record GetThingByIdQuery
{
    public required long Id { get; init; }
}

public sealed class GetThingByIdQueryHandler(AppDbContext context) : IQueryHandler<GetThingByIdQuery, ThingModel>
{
    public async Task<Result<ThingModel>> Handle(GetThingByIdQuery query, CancellationToken cancellationToken)
    {
        var thing = await context.Things.AsNoTracking().FirstOrDefaultAsync(t => t.Id == query.Id, cancellationToken);
        return thing?.MapToModel() ?? Result.Failure<ThingModel>("Thing Not Found");
    }
}