using DotNetStarterProjectTemplate.Application.Features.Things;

namespace DotNetStarterProjectTemplate.Tests.Features.Things;

public sealed class CreateThingCommandHandlerTests
{
    [Test]
    public async Task Handle_CreatesThingAndReturnsModel()
    {
        using var fixture = new DatabaseFixture();
        var handler = new CreateThingCommandHandler(fixture.Context);

        var result = await handler.Handle(new CreateThingCommand { Name = "New Thing" }, CancellationToken.None);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.Name).IsEqualTo("New Thing");
        await Assert.That(result.Value.Id).IsGreaterThan(0L);
    }

    [Test]
    public async Task Handle_PersistsThing_InDatabase()
    {
        using var fixture = new DatabaseFixture();
        var handler = new CreateThingCommandHandler(fixture.Context);

        await handler.Handle(new CreateThingCommand { Name = "Persisted Thing" }, CancellationToken.None);

        await Assert.That(fixture.Context.Things.Count()).IsEqualTo(1);
    }
}
