using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProcHub.Domain.ShippingTerms;

namespace ProcHub.Infrastructure.Persistence.Configurations.Suppliers;

public sealed class ShippingTermConfiguration
    : IEntityTypeConfiguration<ShippingTerm>
{
    public void Configure(EntityTypeBuilder<ShippingTerm> builder)
    {
        builder.ToTable("ShippingTerms");

        builder.HasKey(s => s.Id);


        builder.HasIndex(s => s.Name)
            .IsUnique();
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(ShippingTermConstants.NameMaxLength);
        
        builder.Property(s => s.Description)
            .IsRequired()
            .HasMaxLength(ShippingTermConstants.DescriptionMaxLength);
    }
}