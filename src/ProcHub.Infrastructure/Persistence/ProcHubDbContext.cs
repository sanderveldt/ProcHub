using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ProcHub.Infrastructure.Identity;
using ProcHub.Domain.Suppliers;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.ShippingTerms;
using ProcHub.Application.Abstractions;
using ProcHub.Domain.PurchaseOrders;

namespace ProcHub.Infrastructure.Persistence;

public class ProcHubContext(
    DbContextOptions<ProcHubContext> options)
    : IdentityDbContext<
        ApplicationUser,
        IdentityRole<int>,
        int>(options),
    IProcHubDbContext
{
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<PaymentTerm> PaymentTerms => Set<PaymentTerm>();
    public DbSet<ShippingTerm> ShippingTerms => Set<ShippingTerm>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ProcHubContext).Assembly);      
    }
}