using Microsoft.EntityFrameworkCore;
using ProcHub.Domain.PaymentTerms;

namespace ProcHub.Infrastructure.Persistence.Seeding;

public static class PaymentTermSeeder
{
    public static async Task SeedAsync(
        ProcHubContext dbContext,
        CancellationToken cancellationToken = default)
    {
        if (await dbContext.PaymentTerms
            .AnyAsync(cancellationToken))
        {
            return;
        }

        var paymentTerms = new List<PaymentTerm>
        {
            new PaymentTerm(
                "Pre-payment",
                1m,
                PaymentTimings.On,
                PaymentDateReference.OrderDate,
                0),

            new PaymentTerm(
                "30 days after invoice.",
                0m,
                PaymentTimings.After,
                PaymentDateReference.InvoiceDate,
                30),
            new PaymentTerm(
                "60 days after invoice.",
                0m,
                PaymentTimings.After,
                PaymentDateReference.InvoiceDate,
                60),
            new PaymentTerm(
                "90 days after invoice.",
                0m,
                PaymentTimings.After,
                PaymentDateReference.InvoiceDate,
                90),

            new PaymentTerm(
                "30 days after Bill of Lading.",
                0m,
                PaymentTimings.After,
                PaymentDateReference.BillOfLadingDate,
                30),
            new PaymentTerm(
                "60 days after Bill of Lading.",
                0m,
                PaymentTimings.After,
                PaymentDateReference.BillOfLadingDate,
                60),
            new PaymentTerm(
                "90 days after Bill of Lading.",
                0m,
                PaymentTimings.After,
                PaymentDateReference.BillOfLadingDate,
                90),

            new PaymentTerm(
                "10% deposit, 30 days after invoice.",
                0.1m,
                PaymentTimings.After,
                PaymentDateReference.InvoiceDate,
                30),
            new PaymentTerm(
                "20% deposit, 30 days after invoice.",
                0.2m,
                PaymentTimings.After,
                PaymentDateReference.InvoiceDate,
                30),
            new PaymentTerm(
                "30% deposit, 30 days after invoice.",
                0.3m,
                PaymentTimings.After,
                PaymentDateReference.InvoiceDate,
                30),
            
            new PaymentTerm(
                "10% deposit, 60 days after invoice.",
                0.1m,
                PaymentTimings.After,
                PaymentDateReference.InvoiceDate,
                60),
            new PaymentTerm(
                "20% deposit, 60 days after invoice.",
                0.2m,
                PaymentTimings.After,
                PaymentDateReference.InvoiceDate,
                60),
            new PaymentTerm(
                "30% deposit, 60 days after invoice.",
                0.3m,
                PaymentTimings.After,
                PaymentDateReference.InvoiceDate,
                60),
            
            new PaymentTerm(
                "10% deposit, 30 days after Bill of Lading.",
                0.1m,
                PaymentTimings.After,
                PaymentDateReference.BillOfLadingDate,
                30),
            new PaymentTerm(
                "20% deposit, 30 days after Bill of Lading.",
                0.2m,
                PaymentTimings.After,
                PaymentDateReference.BillOfLadingDate,
                30),
            new PaymentTerm(
                "30% deposit, 30 days after Bill of Lading.",
                0.3m,
                PaymentTimings.After,
                PaymentDateReference.BillOfLadingDate,
                30),
            
             new PaymentTerm(
                "10% deposit, 60 days after Bill of Lading.",
                0.1m,
                PaymentTimings.After,
                PaymentDateReference.BillOfLadingDate,
                60),
            new PaymentTerm(
                "20% deposit, 60 days after Bill of Lading.",
                0.2m,
                PaymentTimings.After,
                PaymentDateReference.BillOfLadingDate,
                60),
            new PaymentTerm(
                "30% deposit, 60 days after Bill of Lading.",
                0.3m,
                PaymentTimings.After,
                PaymentDateReference.BillOfLadingDate,
                60)
        };

        await dbContext.PaymentTerms
            .AddRangeAsync(
                paymentTerms,
                cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}