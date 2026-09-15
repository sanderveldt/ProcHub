using ProcHub.Wpf.Infrastructure.Navigation;

namespace ProcHub.Wpf.Features.Home.ViewModels;

public sealed class HomeViewModel : PageViewModel
{
    public HomeViewModel()
        : base(
            "Home",
            "The ProcHub home view.")
    {
    }
}