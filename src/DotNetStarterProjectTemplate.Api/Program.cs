using DotNetStarterProjectTemplate.Api.Configuration;
using DotNetStarterProjectTemplate.Api.Weather;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddApplicationServicesConfiguration();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapDefaultEndpoints();
app.MapWeatherEndpoints();

app.Run();