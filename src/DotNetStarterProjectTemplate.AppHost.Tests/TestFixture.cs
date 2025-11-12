using Aspire.Hosting;
using DotNetStarterProjectTemplate.Application.Shared;
using Microsoft.Extensions.Logging;
using TUnit.Core.Interfaces;

namespace DotNetStarterProjectTemplate.AppHost.Tests
{
    public sealed class TestFixture : IAsyncInitializer, IAsyncDisposable
    {
        private const string ApiProjectName = $"{Constants.AppAbbreviation}-api";
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
        public DistributedApplication? App;

        public async Task InitializeAsync()
        {
            var appHost = await DistributedApplicationTestingBuilder
                .CreateAsync<Projects.DotNetStarterProjectTemplate_AppHost>();

            appHost.Services.AddLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Debug);

                logging.AddFilter(appHost.Environment.ApplicationName, LogLevel.Debug);
                logging.AddFilter("Aspire.", LogLevel.Debug);
            });

            appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });

            App = await appHost.BuildAsync();
            var resourceNotificationService = App.Services.GetRequiredService<ResourceNotificationService>();
            await App.StartAsync().WaitAsync(DefaultTimeout);

            await resourceNotificationService
                .WaitForResourceAsync(ApiProjectName, KnownResourceStates.Running)
                .WaitAsync(DefaultTimeout);

            await App.StartAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await App!.StopAsync();
        }
    }
}
