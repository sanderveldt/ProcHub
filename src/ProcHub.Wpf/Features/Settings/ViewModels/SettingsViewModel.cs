using ProcHub.Wpf.Infrastructure.Navigation;

namespace ProcHub.Wpf.Features.Settings.ViewModels;

public sealed class SettingsViewModel : PageViewModel
{
    public SettingsViewModel()
        : base("Settings", "Application and client settings.")
    {
    }
}
