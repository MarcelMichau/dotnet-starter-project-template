using DotNetStarterProjectTemplate.Application.Shared;

var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<IResourceWithConnectionString> database;

if (builder.ExecutionContext.IsRunMode)
{
    ParameterResourceBuilderExtensions.CreateDefaultPasswordParameter(builder, "sql-server-password");

    database = builder.AddSqlServer("sql-server")
        .WithDataVolume()
        .AddDatabase("database");
}
else
{
    database = builder.AddAzureSqlServer("sql-server")
        .AddDatabase("database");
}

builder.AddProject<Projects.DotNetStarterProjectTemplate_Api>($"{Constants.AppAbbreviation}-api")
    .WithReference(database)
    .WaitFor(database)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.DotNetStarterProjectTemplate_Worker>($"{Constants.AppAbbreviation}-worker")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();
