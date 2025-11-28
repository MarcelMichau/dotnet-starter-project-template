#pragma warning disable ASPIREPIPELINES001
#pragma warning disable ASPIREPIPELINES004

using System.Globalization;
using System.IO;
using System.Text;
using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Pipelines;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotNetStarterProjectTemplate.AppHost.Annotations;

internal static class SqlResourceBuilderExtensions
{
  public static IResourceBuilder<T> WithProvisioningRequestEmail<T>(
      this IResourceBuilder<T> builder,
      string sqlServerResourceName,
      string adminTeamEmail = "dba-team@example.com",
      string relativeOutputPath = "publish-outbox",
      string? requestingEngineerEmail = null)
      where T : class, IResourceWithConnectionString
  {
    var annotation = new SqlProvisioningRequestAnnotation(
        sqlServerResourceName,
        builder.Resource.Name,
        relativeOutputPath,
        adminTeamEmail,
        requestingEngineerEmail);

    builder.WithAnnotation(annotation);

    builder.WithPipelineStepFactory(_ =>
    {
      var step = new PipelineStep
      {
        Name = $"publish-sql-request-{annotation.SqlDatabaseResourceName}",
        Resource = builder.Resource,
        Tags = ["sql-provisioning"],
        Action = async context =>
            {
              await SqlProvisioningRequestPublisher.WriteAsync(context, annotation);
            }
      };

      step.DependsOn(WellKnownPipelineSteps.PublishPrereq);
      step.RequiredBy(WellKnownPipelineSteps.Publish);

      return step;
    });

    return builder;
  }
}

internal static class SqlProvisioningRequestPublisher
{
  public static async Task WriteAsync(PipelineStepContext context, SqlProvisioningRequestAnnotation annotation)
  {
    var outputService = context.Services.GetRequiredService<IPipelineOutputService>();
    var outputRoot = outputService.GetOutputDirectory();
    var targetDirectory = Path.Combine(outputRoot, annotation.OutputDirectoryName);
    Directory.CreateDirectory(targetDirectory);

    var hostEnvironment = context.Services.GetService<IHostEnvironment>();
    var environmentName = hostEnvironment?.EnvironmentName ?? "Production";
    var timestamp = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture);
    var filePath = Path.Combine(targetDirectory, annotation.FileName);

    var builder = new StringBuilder();
    builder.AppendLine($"To: {annotation.AdminTeamEmail}");
    if (!string.IsNullOrWhiteSpace(annotation.RequestingEngineerEmail))
    {
      builder.AppendLine($"Cc: {annotation.RequestingEngineerEmail}");
    }

    builder.AppendLine($"Subject: SQL provisioning request - {annotation.SqlDatabaseResourceName}");
    builder.AppendLine();
    builder.AppendLine("Hello DBA team,");
    builder.AppendLine();
    builder.AppendLine("Please provision the following SQL assets as part of the upcoming deployment:");
    builder.AppendLine($"  - Azure SQL Server resource: {annotation.SqlServerResourceName}");
    builder.AppendLine($"  - Azure SQL Database resource: {annotation.SqlDatabaseResourceName}");
    builder.AppendLine($"  - Target environment: {environmentName}");
    builder.AppendLine($"  - Requested on (UTC): {timestamp}");
    builder.AppendLine();
    builder.AppendLine("Once provisioned, please confirm the connection details so we can update the corresponding Aspire secrets.");
    builder.AppendLine();
    builder.AppendLine("Many thanks,");
    builder.AppendLine("Aspire automation");

    await File.WriteAllTextAsync(filePath, builder.ToString(), context.CancellationToken);
    context.Logger.LogInformation("SQL provisioning request written to {FilePath}", filePath);
  }
}

#pragma warning restore ASPIREPIPELINES004
#pragma warning restore ASPIREPIPELINES001
