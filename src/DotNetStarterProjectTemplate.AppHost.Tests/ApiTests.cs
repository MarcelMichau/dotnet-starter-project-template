using DotNetStarterProjectTemplate.Application.Shared;

namespace DotNetStarterProjectTemplate.AppHost.Tests;

public sealed class ApiTests
{
    private const string ApiProjectName = $"{Constants.Key}-api";

    [Fact]
    public async Task GetThingsReturnsOkStatusCode()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.DotNetStarterProjectTemplate_AppHost>();

        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging
        await using var app = await appHost.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        // Act
        var httpClient = app.CreateHttpClient(ApiProjectName);

        await resourceNotificationService
            .WaitForResourceAsync(ApiProjectName, KnownResourceStates.Running)
            .WaitAsync(TimeSpan.FromSeconds(30));

        var response = await httpClient.GetAsync("/api/things");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}