using DotNetStarterProjectTemplate.Application.Domain.Things;
using DotNetStarterProjectTemplate.Application.Features.Things;

namespace DotNetStarterProjectTemplate.Tests.Features.Things;

public sealed class DeleteThingCommandHandlerTests
{
    [Test]
    public async Task Handle_ReturnsFailure_WhenThingDoesNotExist()
    {
        using var fixture = new DatabaseFixture();
        var handler = new DeleteThingCommandHandler(fixture.Context);

        var result = await handler.Handle(new DeleteThingCommand { Id = -1 }, CancellationToken.None);

        await Assert.That(result.IsFailure).IsTrue();
    }

    [Test]
    public async Task Handle_DeletesThing_WhenThingExists()
    {
        using var fixture = new DatabaseFixture();
        var thing = new Thing { Name = "To Be Deleted" };
        fixture.Context.Things.Add(thing);
        await fixture.Context.SaveChangesAsync();

        var handler = new DeleteThingCommandHandler(fixture.Context);
        var result = await handler.Handle(new DeleteThingCommand { Id = thing.Id }, CancellationToken.None);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(fixture.Context.Things.Any()).IsFalse();
    }
}
