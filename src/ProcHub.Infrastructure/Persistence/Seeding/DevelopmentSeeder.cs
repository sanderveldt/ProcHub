using Microsoft.EntityFrameworkCore;
using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.PurchaseOrders.Enums;
using ProcHub.Domain.Suppliers;

namespace ProcHub.Infrastructure.Persistence.Seeding;

public static class DevelopmentSeeding
{
    public static async Task SeedDevelopmentAsync(
        ProcHubContext dbContext,
        CancellationToken cancellationToken = default)
    {
        await SeedSuppliersAsync(
            dbContext, 
            cancellationToken);

        await SeedPurchaseOrdersAsync(
            dbContext, 
            cancellationToken);
    }

    public static async Task SeedSuppliersAsync(
        ProcHubContext dbContext,
        CancellationToken cancellationToken = default)
    {
        if (await dbContext.Suppliers
            .AnyAsync(cancellationToken))
        {
            return;
        }

        var prePaymentTerm = await dbContext
            .PaymentTerms.SingleAsync(
                pt => pt.Description == "Pre-payment");
        
        var payment60Days = await dbContext
            .PaymentTerms.SingleAsync(
                pt => pt.Description == 
                    "60 days after Bill of Lading.");
        
        var payment20Deposit = await dbContext
            .PaymentTerms.SingleAsync(
                pt => pt.Description == 
                    "20% deposit, 30 days after invoice.");
        
        var shippingTerm = await dbContext
            .ShippingTerms.SingleAsync(
                st => st.Name == "FOB");


        var suppliers = new List<Supplier>
        {
            new Supplier(
                "TestSupp1",
                "TestSupplier1",
                prePaymentTerm,
                shippingTerm),
            
            new Supplier(
                "TestSupp2",
                "TestSupplier2",
                payment60Days,
                shippingTerm),
            
            new Supplier(
                "TestSupp3",
                "TestSupplier3",
                payment20Deposit,
                shippingTerm)
        };

        await dbContext.Suppliers
            .AddRangeAsync(
                suppliers,
                cancellationToken);
        
        await dbContext
            .SaveChangesAsync(cancellationToken);
    }

    public static async Task SeedPurchaseOrdersAsync(
        ProcHubContext dbContext,
        CancellationToken cancellationToken = default)
    {
        if (await dbContext.PurchaseOrders
            .AnyAsync(cancellationToken))
        {
            return;
        }

        var supplier1 = await dbContext
            .Suppliers.SingleAsync(
                s => s.Code == "TestSupp1");

        
        var supplier2 = await dbContext
            .Suppliers.SingleAsync(
                s => s.Code == "TestSupp2");
        
        var payment30Days = await dbContext
            .PaymentTerms.SingleAsync(
                pt => pt.Description ==
                    "30 days after invoice.");

        supplier2.SetSecondaryPaymentTerm(payment30Days);


        var supplier3 = await dbContext
            .Suppliers.SingleAsync(
                s => s.Code == "TestSupp3");
        
        var payment60Days = await dbContext
            .PaymentTerms.SingleAsync(
                pt => pt.Description ==
                    "60 days after invoice.");
        
        supplier3.SetSamplePaymentTerm(payment60Days);


        var purchaseOrders = new List<PurchaseOrder>
        {
            new PurchaseOrder(
                "TestPO1",
                PurchaseOrderType.Regular,
                supplier1),
            
            new PurchaseOrder(
                "TestPO2",
                PurchaseOrderType.Express,
                supplier2),
            
            new PurchaseOrder(
                "TestPO3",
                PurchaseOrderType.Sample,
                supplier3)
        };

        await dbContext.PurchaseOrders
            .AddRangeAsync(
                purchaseOrders,
                cancellationToken);
        
        await dbContext
            .SaveChangesAsync(cancellationToken);
    }
}