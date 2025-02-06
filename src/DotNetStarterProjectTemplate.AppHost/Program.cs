using DotNetStarterProjectTemplate.Application.Shared;

var builder = DistributedApplication.CreateBuilder(args);

ParameterResourceBuilderExtensions.CreateDefaultPasswordParameter(builder, "sql-server-password");

var database = builder.AddSqlServer("sql-server")
    .WithDataVolume()
    .AddDatabase("database");

builder.AddProject<Projects.DotNetStarterProjectTemplate_Api>($"{Constants.Key}-api")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Projects.DotNetStarterProjectTemplate_Worker>($"{Constants.Key}-worker")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();
