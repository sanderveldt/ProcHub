using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProcHub.Domain.Suppliers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ProcHub.Infrastructure.Persistence.Configurations.Suppliers;

public sealed class PaymentTermConfiguration
    : IEntityTypeConfiguration<PaymentTerm>
{
    public void Configure(EntityTypeBuilder<PaymentTerm> builder)
    {
        builder.ToTable("PaymentTerms");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(p => p.DepositPercentage)
            .IsRequired()
            .HasPrecision(4, 3)
            .HasDefaultValue(0);

        builder.Property(p => p.PaymentTiming)
            .IsRequired();

        builder.Property(p => p.DueDateReference)
            .IsRequired();

        builder.Property(p => p.BalanceDueDays)
            .IsRequired()
            .HasDefaultValue(0);

        builder.ToTable(table =>
        {
           table.HasCheckConstraint(
                "CK_PaymentTerms_DepositPercentage",
                "\"DepositPercentage\" >= 0 AND \"DepositPercentage\" <= 1");

            table.HasCheckConstraint(
                "CK_PaymentTerms_BalanceDueDays",
                "\"BalanceDueDays\" >= 0");
        });
    }
}