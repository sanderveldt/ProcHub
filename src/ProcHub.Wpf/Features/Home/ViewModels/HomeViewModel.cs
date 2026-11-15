using ProcHub.Wpf.Infrastructure.Api.Clients;
using ProcHub.Wpf.Infrastructure.Navigation;

namespace ProcHub.Wpf.Features.Home.ViewModels;

public sealed class HomeViewModel : PageViewModel
{
    private readonly UsersApiClient _usersApiClient;

    public HomeViewModel(
        UsersApiClient usersApiClient)
        : base("Home", "The ProcHub home view.")
    {
        _usersApiClient = usersApiClient;
    }
}