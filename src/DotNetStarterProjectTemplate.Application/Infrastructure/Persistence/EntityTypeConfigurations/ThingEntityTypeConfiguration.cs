using Microsoft.EntityFrameworkCore;
using DotNetStarterProjectTemplate.Application.Domain.Things;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetStarterProjectTemplate.Application.Infrastructure.Persistence.EntityTypeConfigurations;

internal sealed class ThingEntityTypeConfiguration: IEntityTypeConfiguration<Thing>
{
    public void Configure(EntityTypeBuilder<Thing> builder)
    {
        const string tableName = "Thing";

        builder
            .ToTable(tableName, AppDbContext.DefaultSchema);

        builder
            .HasKey(s => s.Id);

        builder
            .Property(s => s.Name)
            .HasMaxLength(250)
            .IsRequired();
    }
}