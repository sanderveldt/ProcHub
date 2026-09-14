using Microsoft.Extensions.DependencyInjection;

namespace ProcHub.Wpf.Infrastructure.Navigation;

public sealed class NavigationService(IServiceProvider serviceProvider)
    : ObservableObject, INavigationService
{
    private PageViewModel? _currentViewModel;

    public PageViewModel? CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    public void NavigateTo<TViewModel>()
        where TViewModel : PageViewModel
    {
        CurrentViewModel = serviceProvider.GetRequiredService<TViewModel>();
    }
}