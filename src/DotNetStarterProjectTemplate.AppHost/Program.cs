using DotNetStarterProjectTemplate.Application.Shared;

var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<IResourceWithConnectionString> database;

if (builder.ExecutionContext.IsRunMode)
{
    database = builder.AddSqlServer("sql-server")
        .WithDataVolume()
        .AddDatabase("database");
}
else
{
    database = builder.AddAzureSqlServer("sql-server")
        .AddDatabase("database");
}

var worker = builder.AddProject<Projects.DotNetStarterProjectTemplate_Worker>($"{Constants.AppAbbreviation}-worker")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Projects.DotNetStarterProjectTemplate_Api>($"{Constants.AppAbbreviation}-api")
    .WithReference(database)
    .WaitFor(worker)
    .WithExternalHttpEndpoints();

builder.Build().Run();
