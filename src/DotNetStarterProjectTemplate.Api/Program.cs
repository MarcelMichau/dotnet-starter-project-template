using DotNetStarterProjectTemplate.Api.Configuration;
using DotNetStarterProjectTemplate.Api.Things;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddApplicationServicesConfiguration();

builder.AddOpenApiConfiguration();

var app = builder.Build();

app.MapOpenApiConfiguration();

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapDefaultEndpoints();
app.MapThingEndpoints();

app.Run();