using DotNetStarterProjectTemplate.Application.Domain.Things;
using DotNetStarterProjectTemplate.Application.Features.Things;

namespace DotNetStarterProjectTemplate.Tests.Features.Things;

public sealed class ThingMappingTests
{
    [Test]
    public async Task MapToModel_MapsIdCorrectly()
    {
        var thing = new Thing { Id = 42, Name = "Test Thing" };

        var model = thing.MapToModel();

        await Assert.That(model.Id).IsEqualTo(42L);
    }

    [Test]
    public async Task MapToModel_MapsNameCorrectly()
    {
        var thing = new Thing { Id = 1, Name = "My Thing" };

        var model = thing.MapToModel();

        await Assert.That(model.Name).IsEqualTo("My Thing");
    }
}
