using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProcHub.Domain.Suppliers;

namespace ProcHub.Infrastructure.Persistence.Configurations.Suppliers;

public sealed class SupplierConfiguration
    : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();

        builder.Property(s => s.CreationDate)
            .IsRequired();

         builder.HasIndex(s => s.Code)
            .IsUnique();
        builder.Property(s => s.Code)
            .IsRequired()
            .HasMaxLength(SupplierConstants.CodeMaxLength);
        
        builder.HasIndex(s => s.Name);
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(SupplierConstants.NameMaxLength);

        builder.Property(s => s.FullName)
            .HasMaxLength(SupplierConstants.FullNameMaxLength);


        builder.HasOne(x => x.MainShippingTerm)
            .WithMany()
            .HasForeignKey(x => x.MainShippingTermId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SecondaryShippingTerm)
            .WithMany()
            .HasForeignKey(x => x.SecondaryShippingTermId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SampleShippingTerm)
            .WithMany()
            .HasForeignKey(x => x.SampleShippingTermId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasOne(x => x.DefaultPaymentTerm)
            .WithMany()
            .HasForeignKey(x => x.DefaultPaymentTermId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SecondaryPaymentTerm)
            .WithMany()
            .HasForeignKey(x => x.SecondaryPaymentTermId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

