using DotNetStarterProjectTemplate.Application.Shared;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    public const string DefaultSchema = Constants.Name;

    public AppDbContext(DbContextOptions options) : base(options)
    {
        ArgumentNullException.ThrowIfNull(options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}