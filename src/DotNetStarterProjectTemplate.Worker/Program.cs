using DotNetStarterProjectTemplate.Worker.Configuration;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults()
    .AddApplicationServicesConfiguration();

var host = builder.Build();
host.Run();
