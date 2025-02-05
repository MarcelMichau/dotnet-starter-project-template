using DotNetStarterProjectTemplate.Application.Domain.Things;

namespace DotNetStarterProjectTemplate.Api.Things;

public sealed class ThingModel
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
