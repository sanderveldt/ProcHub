using Microsoft.Extensions.DependencyInjection;
using ProcHub.Wpf.Infrastructure.Authentication;

namespace ProcHub.Wpf.Infrastructure.Navigation;

public sealed class NavigationService(
    IServiceProvider serviceProvider,
    AuthSession authSession)
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
        if (!authSession.IsAuthenticated)
        {
            throw new UnauthorizedAccessException(
                "Unauthorized user.");
        }

        var viewModel =
            serviceProvider.GetRequiredService<TViewModel>();

        if (!viewModel.IsAllowedFor(authSession.Role))
        {
            throw new UnauthorizedAccessException(
                $"Role '{authSession.Role}' is not allowed to access '{viewModel.Title}'.");
        }

        CurrentViewModel = viewModel;
    }
}