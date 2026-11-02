using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcHub.Domain.Countries;

namespace ProcHub.Infrastructure.Persistence.Configurations;

public sealed class CountryConfiguration
    : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Countries");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(CountryConstants.CodeMaxLength);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(CountryConstants.NameMaxLength);

        builder.HasIndex(c => c.Code)
            .IsUnique();
    }
}