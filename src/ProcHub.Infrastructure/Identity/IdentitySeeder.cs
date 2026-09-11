using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ProcHub.Application.Authorization;

namespace ProcHub.Infrastructure.Identity;

public sealed class IdentitySeeder(
    RoleManager<IdentityRole<int>> roleManager,
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration)
{
    public async Task SeedAsync()
    {
        await CreateRoleIfMissing(AppRoles.User);
        await CreateRoleIfMissing(AppRoles.SuperUser);
        await CreateRoleIfMissing(AppRoles.Admin);

        await CreateAdminIfMissing();
    }

    private async Task CreateRoleIfMissing(string roleName)
    {
        if (await roleManager.RoleExistsAsync(roleName))
        {
            return;
        }

        var result = await roleManager.CreateAsync(
            new IdentityRole<int>(roleName));
        
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                $"Couldn't create role '{roleName}'.");
        }
    }

    private async Task CreateAdminIfMissing()
    {
        var email = configuration["InitialAdmin:Email"];
        var password = configuration["InitialAdmin:Password"];

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Initial Admin credentials not configured.");
        }

        email = email.Trim();

        var existingAdmin = await userManager
            .FindByEmailAsync(email);

        if (existingAdmin is not null)
        {
            if (!await userManager.IsInRoleAsync(
                existingAdmin,
                AppRoles.Admin))
            {
                var addRoleResult = await userManager
                    .AddToRoleAsync(
                        existingAdmin,
                        AppRoles.Admin);
                
                if (!addRoleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Couldn't assign the Admin role to initial Admin.");
                }
            }

            return;
        }

        var admin = new ApplicationUser
        {
            UserName = email,
            Email = email,
            DisplayName = "Administrator"
        };

         var createResult =
            await userManager.CreateAsync(
                admin,
                password);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(
                ", ",
                createResult.Errors
                    .Select(error => error.Description));

            throw new InvalidOperationException(
                $"Couldn't create initial Admin: {errors}");
        }

        var roleResult =
            await userManager.AddToRoleAsync(
                admin,
                AppRoles.Admin);

        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(admin);

            throw new InvalidOperationException(
                "Couldn't assign Admin role to initial Admin.");
        }
    }
}