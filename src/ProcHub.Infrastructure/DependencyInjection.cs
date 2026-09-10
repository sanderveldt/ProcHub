using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProcHub.Application.Abstractions;
using ProcHub.Infrastructure.Identity;
using ProcHub.Infrastructure.Persistence;

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
            options.UseSqlite(connectionString));
        
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

        services.AddScoped<IdentitySeeder>();
        
        return services;
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
}