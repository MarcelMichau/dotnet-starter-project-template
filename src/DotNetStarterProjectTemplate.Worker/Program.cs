using DotNetStarterProjectTemplate.Worker;
using DotNetStarterProjectTemplate.Worker.Configuration;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults()
    .AddApplicationServicesConfiguration();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
