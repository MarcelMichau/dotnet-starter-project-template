using Aspire.Hosting;
using DotNetStarterProjectTemplate.Application.Shared;
using TUnit.Core.Interfaces;

namespace DotNetStarterProjectTemplate.AppHost.Tests
{
    public sealed class TestFixture : IAsyncInitializer, IAsyncDisposable
    {
        private const string ApiProjectName = $"{Constants.AppAbbreviation}-api";
        public DistributedApplication? App;

        public async Task InitializeAsync()
        {
            var appHost = await DistributedApplicationTestingBuilder
                .CreateAsync<Projects.DotNetStarterProjectTemplate_AppHost>();

            appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });

            App = await appHost.BuildAsync();
            var resourceNotificationService = App.Services.GetRequiredService<ResourceNotificationService>();
            await App.StartAsync();

            await resourceNotificationService
                .WaitForResourceAsync(ApiProjectName, KnownResourceStates.Running)
                .WaitAsync(TimeSpan.FromSeconds(30));

            await App.StartAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await App!.StopAsync();
        }
    }
}
