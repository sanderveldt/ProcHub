namespace ProcHub.Wpf.Infrastructure.Authentication;

public static class AppRoles
{
    public const string User = "User";
    public const string SuperUser = "SuperUser";
    public const string Admin = "Admin";

    public static IReadOnlyList<string> All { get; } =
    [
        User,
        SuperUser,
        Admin
    ];
}