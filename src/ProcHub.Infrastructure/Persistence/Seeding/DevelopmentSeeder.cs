using Microsoft.EntityFrameworkCore;
using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.Suppliers;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.ShippingTerms;
using ProcHub.Infrastructure.Persistence;

namespace ProcHub.Infrastructure.Persistence.Seeding;

public static class DevelopmentSeeding
{
    public static async Task SeedDevelopmentAsync(
        ProcHubContext dbContext,
        CancellationToken cancellationToken)
    {
        
    }
}