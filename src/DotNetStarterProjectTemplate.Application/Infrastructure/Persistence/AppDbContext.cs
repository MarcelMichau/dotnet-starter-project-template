using DotNetStarterProjectTemplate.Application.Shared;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using DotNetStarterProjectTemplate.Application.Domain.Things;

namespace DotNetStarterProjectTemplate.Application.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    public const string DefaultSchema = Constants.AppName;

    public AppDbContext(DbContextOptions options) : base(options)
    {
        ArgumentNullException.ThrowIfNull(options);
    }

    public DbSet<Thing> Things => Set<Thing>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}