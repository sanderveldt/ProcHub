using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ProcHub.Infrastructure.Identity;
using ProcHub.Domain.Suppliers;
using ProcHub.Application.Abstractions;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ProcHubContext).Assembly);      
    }
}