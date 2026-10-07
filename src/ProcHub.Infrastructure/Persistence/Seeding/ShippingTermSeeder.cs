using Microsoft.EntityFrameworkCore;
using ProcHub.Domain.ShippingTerms;

namespace ProcHub.Infrastructure.Persistence.Seeding;

public static class ShippingTermSeeder
{
    public static async Task SeedAsync(
        ProcHubContext dbContext,
        CancellationToken cancellationToken = default)
    {
        if (await dbContext.ShippingTerms
            .AnyAsync(cancellationToken))
        {
            return;
        }

        var shippingTerms = new List<ShippingTerm>
        {
            new ShippingTerm("EXW","Ex Works"),
            new ShippingTerm("FCA", "Free Carrier"),
            new ShippingTerm("CPT", "Carriage Paid To"),
            new ShippingTerm("DAP", "Delivered at Place"),
            new ShippingTerm("DPU", "Delivered at Place Unloaded"),
            new ShippingTerm("DDP", "Delivered Duty Paid"),
            new ShippingTerm("FAS", "Free Alongside Ship"),
            new ShippingTerm("FOB", "Free On Board"),
            new ShippingTerm("CFR", "Cost and Freight"),
            new ShippingTerm("CIF", "Cost, Insurance, and Freight")
        };

        await dbContext.ShippingTerms
            .AddRangeAsync(
                shippingTerms,
                cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }   
}