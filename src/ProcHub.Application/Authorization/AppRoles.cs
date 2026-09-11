namespace ProcHub.Application.Authorization;

public static class AppRoles
{
    public const string User = "User";
    public const string SuperUser = "SuperUser";
    public const string Admin = "Admin";

    public static bool IsValid(string role)
    {
        return role is User or SuperUser or Admin;
    }
}