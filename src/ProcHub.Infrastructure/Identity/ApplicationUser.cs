using Microsoft.AspNetCore.Identity;

namespace ProcHub.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<int>
{
    public string? DisplayName { get; set; } = string.Empty;
}