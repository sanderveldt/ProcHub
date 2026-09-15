using ProcHub.Contracts.Authentication.Responses;

namespace ProcHub.Wpf.Infrastructure.Authentication;

public sealed class AuthSession
{
    public CurrentUserResponse? CurrentUser { get; private set; }
    public string? Role => CurrentUser?.Role;
    public bool IsAuthenticated => CurrentUser is not null;

    public void SetCurrentUser(CurrentUserResponse currentUser)
    {
        CurrentUser = currentUser;
    }

    public void Clear()
    {
        CurrentUser = null;
    }
}