namespace ProcHub.Application.Authorization;

public static class AppPolicies
{
    public const string SuperUserOrAdmin = "SuperUserOrAdmin";
    public const string AdminOnly = "AdminOnly";
}