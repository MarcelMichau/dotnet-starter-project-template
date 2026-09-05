using DotNetStarterProjectTemplate.Application.Domain.Things;
using DotNetStarterProjectTemplate.Application.Features.Things;

namespace DotNetStarterProjectTemplate.Tests.Features.Things;

public sealed class GetThingByIdQueryHandlerTests
{
    [Test]
    public async Task Handle_ReturnsFailure_WhenThingDoesNotExist()
    {
        using var fixture = new DatabaseFixture();
        var handler = new GetThingByIdQueryHandler(fixture.Context);

        var result = await handler.Handle(new GetThingByIdQuery { Id = -1 }, CancellationToken.None);

        await Assert.That(result.IsFailure).IsTrue();
    }

    [Test]
    public async Task Handle_ReturnsThing_WhenThingExists()
    {
        using var fixture = new DatabaseFixture();
        var thing = new Thing { Name = "Existing Thing" };
        fixture.Context.Things.Add(thing);
        await fixture.Context.SaveChangesAsync();

        var handler = new GetThingByIdQueryHandler(fixture.Context);
        var result = await handler.Handle(new GetThingByIdQuery { Id = thing.Id }, CancellationToken.None);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.Name).IsEqualTo("Existing Thing");
    }
}
