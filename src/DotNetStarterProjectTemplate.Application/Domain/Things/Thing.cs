namespace DotNetStarterProjectTemplate.Application.Domain.Things;

public sealed class Thing
{
    public long Id { get; set; }
    public required string Name { get; set; }
}