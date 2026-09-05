using DotNetStarterProjectTemplate.Api.Configuration;
using DotNetStarterProjectTemplate.Api.Things;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddApplicationServicesConfiguration();

builder.AddApiVersioningConfiguration();

builder.Services.AddProblemDetails();
builder.Services.AddRequestTimeouts();
builder.Services.AddOutputCache();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapDefaultEndpoints();
app.MapThingEndpoints();
app.MapOpenApiConfiguration();

app.UseRequestTimeouts();
app.UseOutputCache();

app.Run();