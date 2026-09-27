using Microsoft.EntityFrameworkCore;
using ProcHub.Domain.Suppliers;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.ShippingTerms;
using ProcHub.Domain.PurchaseOrders;

namespace ProcHub.Application.Abstractions;

public interface IProcHubDbContext
{
    DbSet<Supplier> Suppliers { get; }
    DbSet<PaymentTerm> PaymentTerms { get; }
    DbSet<ShippingTerm> ShippingTerms { get; }
    DbSet<PurchaseOrder> PurchaseOrders { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}