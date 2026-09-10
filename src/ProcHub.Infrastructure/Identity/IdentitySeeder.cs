using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using ProcHub.Application.Authorization;

namespace ProcHub.Infrastructure.Identity;

public sealed class IdentitySeeder(
    RoleManager<IdentityRole<int>> roleManager)
{
    public async Task SeedAsync()
    {
        await CreateRoleIfMissing(AppRoles.User);
        await CreateRoleIfMissing(AppRoles.SuperUser);
        await CreateRoleIfMissing(AppRoles.Admin);
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
}