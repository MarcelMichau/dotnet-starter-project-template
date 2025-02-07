using DotNetStarterProjectTemplate.Worker.Configuration;
using DotNetStarterProjectTemplate.Worker.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults()
    .AddApplicationServicesConfiguration();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(DatabaseMigrationHostedService.ActivitySourceName));

var host = builder.Build();
host.Run();
