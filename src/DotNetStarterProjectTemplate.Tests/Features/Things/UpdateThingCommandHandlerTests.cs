using DotNetStarterProjectTemplate.Application.Domain.Things;
using DotNetStarterProjectTemplate.Application.Features.Things;

namespace DotNetStarterProjectTemplate.Tests.Features.Things;

public sealed class UpdateThingCommandHandlerTests
{
    [Test]
    public async Task Handle_ReturnsFailure_WhenThingDoesNotExist()
    {
        using var fixture = new DatabaseFixture();
        var handler = new UpdateThingCommandHandler(fixture.Context);

        var result = await handler.Handle(new UpdateThingCommand { Id = -1, Name = "Updated" }, CancellationToken.None);

        await Assert.That(result.IsFailure).IsTrue();
    }

    [Test]
    public async Task Handle_UpdatesThingName_WhenThingExists()
    {
        using var fixture = new DatabaseFixture();
        var thing = new Thing { Name = "Original" };
        fixture.Context.Things.Add(thing);
        await fixture.Context.SaveChangesAsync();

        var handler = new UpdateThingCommandHandler(fixture.Context);
        var result = await handler.Handle(new UpdateThingCommand { Id = thing.Id, Name = "Updated" }, CancellationToken.None);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.Name).IsEqualTo("Updated");
    }
}
