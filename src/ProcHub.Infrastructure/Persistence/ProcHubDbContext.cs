using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ProcHub.Infrastructure.Identity;
using ProcHub.Domain.Suppliers;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.ShippingTerms;
using ProcHub.Application.Abstractions;
using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.Articles;
using ProcHub.Domain.Articles.ArticleGroups;
using ProcHub.Domain.Countries;

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
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<ArticleGroup> ArticleGroups => Set<ArticleGroup>();
    public DbSet<Country> Countries => Set<Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ProcHubContext).Assembly);      
    }
}