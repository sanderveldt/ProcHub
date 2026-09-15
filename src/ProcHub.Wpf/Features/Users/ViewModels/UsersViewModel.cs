using ProcHub.Wpf.Infrastructure.Authentication;
using ProcHub.Wpf.Infrastructure.Navigation;

namespace ProcHub.Wpf.Features.Users.ViewModels;

public sealed class UsersViewModel : PageViewModel
{
    public UsersViewModel()
        : base(
            "Users",
            "Maintain users and user settings.",
            AppRoles.SuperUser,
            AppRoles.Admin)
    {
    }
}