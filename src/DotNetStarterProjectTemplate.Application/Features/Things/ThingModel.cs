using DotNetStarterProjectTemplate.Application.Domain.Things;

namespace DotNetStarterProjectTemplate.Application.Features.Things;

public sealed record ThingModel
{
    public required long Id { get; init; }
    public required string Name { get; init; }
}

public static class ThingMappingExtensions
{
    public static ThingModel MapToModel(this Thing thing)
    {
        return new ThingModel
        {
            Id = thing.Id,
            Name = thing.Name,
        };
    }
}
