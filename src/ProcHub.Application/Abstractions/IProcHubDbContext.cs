using Microsoft.EntityFrameworkCore;
using ProcHub.Domain.Suppliers;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.ShippingTerms;
using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.Articles;
using ProcHub.Domain.Articles.ArticleGroups;
using ProcHub.Domain.Countries;

namespace ProcHub.Application.Abstractions;

public interface IProcHubDbContext
{
    DbSet<Supplier> Suppliers { get; }
    DbSet<PaymentTerm> PaymentTerms { get; }
    DbSet<ShippingTerm> ShippingTerms { get; }
    DbSet<PurchaseOrder> PurchaseOrders { get; }
    DbSet<Article> Articles { get; }
    DbSet<ArticleGroup> ArticleGroups { get; }
    DbSet<Country> Countries { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}