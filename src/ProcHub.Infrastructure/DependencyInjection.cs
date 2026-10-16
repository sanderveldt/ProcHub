using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProcHub.Application.Abstractions;
using ProcHub.Infrastructure.Identity;
using ProcHub.Infrastructure.Persistence;
using ProcHub.Infrastructure.Persistence.Seeding;

namespace ProcHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("ProcHubDb")
            ?? throw new InvalidOperationException(
                "Connecting string 'ProcHubDb' not found.");

        services.AddDbContext<ProcHubContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddScoped<IProcHubDbContext>(provider =>
            provider.GetRequiredService<ProcHubContext>());

        services.AddIdentityApiEndpoints<ApplicationUser>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan =
                TimeSpan.FromMinutes(15);
        })
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<ProcHubContext>();

        services.AddScoped<
            IUserIdentityService,
            UserIdentityService>();
            
        services.AddScoped<IdentitySeeder>();
        
        return services;
    }

    public static async Task SeedDatabaseAsync(
        this IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        await using var scope = serviceProvider
            .CreateAsyncScope();
        
        var context = scope.ServiceProvider
            .GetRequiredService<ProcHubContext>();

        await using var transaction =
            await context.Database
                .BeginTransactionAsync(cancellationToken);     
        
        await ShippingTermSeeder.SeedAsync(
            context, cancellationToken);
        
        await PaymentTermSeeder.SeedAsync(
            context, cancellationToken);
        
        await transaction.CommitAsync(cancellationToken);
    }

    public static async Task InitializeDatabaseAsync(
        this IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var context = scope.ServiceProvider
            .GetRequiredService<ProcHubContext>();
        
        await context.Database.MigrateAsync(cancellationToken);
    }

    public static async Task InitializeIdentityAsync(
        this IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider
            .CreateAsyncScope();

        var seeder = scope.ServiceProvider
            .GetRequiredService<IdentitySeeder>();
        
        await seeder.SeedAsync();
    }

    public static async Task SeedDevelopmentAsync( 
        this IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        await using var scope = serviceProvider 
            .CreateAsyncScope();
        
        var dbContext = scope.ServiceProvider
            .GetRequiredService<ProcHubContext>();
        
        await DevelopmentSeeding
            .SeedDevelopmentAsync(
                dbContext,
                cancellationToken);
    }
}