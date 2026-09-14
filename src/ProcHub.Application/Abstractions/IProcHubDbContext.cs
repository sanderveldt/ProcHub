using Microsoft.EntityFrameworkCore;
using ProcHub.Domain.Suppliers;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.ShippingTerms;

namespace ProcHub.Application.Abstractions;

public interface IProcHubDbContext
{
    DbSet<Supplier> Suppliers { get; }
    DbSet<PaymentTerm> PaymentTerms { get; }
    DbSet<ShippingTerm> ShippingTerms { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}