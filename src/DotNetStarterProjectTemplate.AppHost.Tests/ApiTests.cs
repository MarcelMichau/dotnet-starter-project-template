using System.Net.Http.Json;
using DotNetStarterProjectTemplate.Application.Features.Things;
using DotNetStarterProjectTemplate.Application.Shared;

namespace DotNetStarterProjectTemplate.AppHost.Tests;

public sealed class ApiTests
{
    [ClassDataSource<TestFixture>(Shared = SharedType.PerTestSession)]
    public required TestFixture TestFixture { get; init; }

    private HttpClient Client => TestFixture.App!.CreateHttpClient(ApiProjectName);

    private const string ApiProjectName = $"{Constants.AppAbbreviation}-api";

    [Test]
    public async Task GetThingsReturnsOkStatusCode()
    {
        // Act
        var response = await Client.GetAsync("/api/v1/things");

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
    }

    [Test]
    public async Task GetThingByIdReturnsOkStatusCode()
    {
        // Arrange
        var createdThingId = await CreateNewThing();

        // Act
        var response = await Client.GetAsync($"/api/v1/things/{createdThingId}");

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
    }

    [Test]
    public async Task GetNonExistentThingByIdReturnsNotFoundStatusCode()
    {
        // Act
        var response = await Client.GetAsync("/api/v1/things/-1");

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task CreateThingReturnsCreatedStatusCode()
    {
        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/things", new { Name = "New Thing" });

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.Created);
    }

    [Test]
    public async Task UpdateThingReturnsOkStatusCode()
    {
        // Arrange
        var createdThingId = await CreateNewThing();

        // Act
        var response = await Client.PutAsJsonAsync($"/api/v1/things/{createdThingId}",
            new { Id = createdThingId, Name = "Updated Thing" });

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
    }

    [Test]
    public async Task UpdateNonExistentThingReturnsNotFoundStatusCode()
    {
        // Act
        var response = await Client.PutAsJsonAsync("/api/v1/things/-1",
            new { Id = -1, Name = "Updated Thing" });

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task UpdateThingWithMismatchedIdReturnsBadRequestStatusCode()
    {
        // Act
        var response = await Client.PutAsJsonAsync("/api/v1/things/1",
            new { Id = 2, Name = "Updated Thing" });

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task DeleteThingReturnsNoContentStatusCode()
    {
        // Arrange
        var createdThingId = await CreateNewThing();

        // Act
        var response = await Client.DeleteAsync($"/api/v1/things/{createdThingId}");

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NoContent);
    }

    [Test]
    public async Task DeleteNonExistentThingReturnsNotFoundStatusCode()
    {
        var response = await Client.DeleteAsync("/api/v1/things/-1");

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }

    private async Task<int> CreateNewThing()
    {
        var createResponse = await Client
            .PostAsJsonAsync("/api/v1/things", new { Name = "New Thing" });
        createResponse.EnsureSuccessStatusCode();
        var createdThing = await createResponse.Content.ReadFromJsonAsync<ThingModel>();
        var createdThingId = (int)createdThing!.Id;
        return createdThingId;
    }
}
