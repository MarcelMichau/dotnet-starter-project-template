using DotNetStarterProjectTemplate.Application.Domain.Things;
using DotNetStarterProjectTemplate.Application.Features.Things;

namespace DotNetStarterProjectTemplate.Tests.Features.Things;

public sealed class GetThingsQueryHandlerTests
{
    [Test]
    public async Task Handle_ReturnsEmptyList_WhenNoThingsExist()
    {
        using var fixture = new DatabaseFixture();
        var handler = new GetThingsQueryHandler(fixture.Context);

        var result = await handler.Handle(new GetThingsQuery(), CancellationToken.None);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value).IsEmpty();
    }

    [Test]
    public async Task Handle_ReturnsAllThings()
    {
        using var fixture = new DatabaseFixture();
        fixture.Context.Things.AddRange(
            new Thing { Name = "Thing A" },
            new Thing { Name = "Thing B" });
        await fixture.Context.SaveChangesAsync();

        var handler = new GetThingsQueryHandler(fixture.Context);
        var result = await handler.Handle(new GetThingsQuery(), CancellationToken.None);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.Count).IsEqualTo(2);
    }
}
