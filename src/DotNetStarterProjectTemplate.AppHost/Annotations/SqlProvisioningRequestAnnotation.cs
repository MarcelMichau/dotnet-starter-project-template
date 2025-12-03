namespace DotNetStarterProjectTemplate.AppHost.Annotations;

internal sealed record SqlProvisioningRequestAnnotation(
    string SqlServerResourceName,
    string SqlDatabaseResourceName,
    string OutputDirectoryName,
    string AdminTeamEmail,
    string? RequestingEngineerEmail) : IResourceAnnotation
{
  public string FileName => $"{SqlDatabaseResourceName}-sql-request.txt";
}
