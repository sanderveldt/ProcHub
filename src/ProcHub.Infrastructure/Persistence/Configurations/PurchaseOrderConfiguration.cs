using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.Suppliers;

namespace ProcHub.Infrastructure.Persistence.Configurations;

public sealed class PurchaseOrderConfiguration
    : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> 
        builder)
    {
        builder.ToTable("PurchaseOrders");

        builder.HasKey(po => po.Id);
        builder.Property(po => po.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(po => po.OrderDate)
            .IsRequired();

        builder.Property(po => po.OrderType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(po => po.OrderStatus)
            .HasConversion<string>()
            .HasMaxLength(100)
            .IsRequired();
        
        builder.HasIndex(po => po.Code)
            .IsUnique();
        builder.Property(po => po.Code)
            .IsRequired()
            .HasMaxLength(PurchaseOrderConstants.CodeMaxLength);
        
        builder.HasOne(po => po.Supplier)
            .WithMany()
            .HasForeignKey(po => po.SupplierId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(po => po.SupplierName)
            .IsRequired()
            .HasMaxLength(SupplierConstants.NameMaxLength);
        
        builder.Property(po => po.SupplierCode)
            .IsRequired()
            .HasMaxLength(SupplierConstants.CodeMaxLength);
        
        builder.HasOne(po => po.PaymentTerm)
            .WithMany()
            .HasForeignKey(po => po.PaymentTermId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(po => po.PaymentTiming)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Property(po => po.DueDateReference)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(po => po.DepositPercentage)
            .IsRequired()
            .HasPrecision(4, 3)
            .HasDefaultValue(0);
        
        builder.Property(po => po.BalanceDueDays)
            .IsRequired();

        builder.HasOne(po => po.ShippingTerm)
            .WithMany()
            .HasForeignKey(po => po.ShippingTermId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(po => po.ConfirmationReference)
            .HasMaxLength(PurchaseOrderConstants
                .ConfirmationRefMaxLength);
        
        builder.Property(po => po.InvoiceReference)
            .HasMaxLength(PurchaseOrderConstants
                .InvoiceRefMaxLength);
        
        builder.Property(po => po.OrderNotes)
            .HasMaxLength(PurchaseOrderConstants
                .OrderNotesMaxLength);
        
        builder.Property(po => po.ShipmentSize)
            .HasMaxLength(PurchaseOrderConstants
                .ShipmentSize);
        
        builder.Property(po => po.ShipmentReference)
            .HasMaxLength(PurchaseOrderConstants
                .ShipmentReference);
    }
}