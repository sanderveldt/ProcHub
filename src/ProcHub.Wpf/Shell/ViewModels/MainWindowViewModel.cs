using System.Collections.ObjectModel;
using System.Windows.Input;
using ProcHub.Wpf.Infrastructure;
using ProcHub.Wpf.Infrastructure.Navigation;
using ProcHub.Wpf.Shell.Models;
using ProcHub.Wpf.Features.Settings.ViewModels;

namespace ProcHub.Wpf.Shell.ViewModels;

public sealed class MainWindowViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private bool _isNavigationCompact;

    public MainWindowViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        Navigation = navigationService;

        ToggleNavigationCommand = new RelayCommand(ToggleNavigation);
        SettingsCommand = NavigateCommand<SettingsViewModel>();
    }

    public INavigationService Navigation { get; }

    public ObservableCollection<NavigationItem> NavigationItems { get; }

    public ICommand ToggleNavigationCommand { get; }
    public ICommand SettingsCommand { get; }

        public bool IsNavigationCompact
    {
        get => _isNavigationCompact;
        private set
        {
            if (!SetProperty(ref _isNavigationCompact, value))
                return;

            OnPropertyChanged(nameof(NavigationWidth));
        }
    }

    public double NavigationWidth => IsNavigationCompact ? 56 : 260;

    public void OpenCompactItem(NavigationItem item)
    {
        if (item.Children.Count > 0)
        {
            IsNavigationCompact = false;
            CollapseAllNavigationItems();
            item.IsExpanded = true;
            return;
        }

        if (item.Command?.CanExecute(null) == true)
            item.Command.Execute(null);
    }

    private ICommand NavigateCommand<TViewModel>()
        where TViewModel : PageViewModel
    {
        return new RelayCommand(
            () => _navigationService.NavigateTo<TViewModel>());
    }

    private void ToggleNavigation()
    {
        IsNavigationCompact = !IsNavigationCompact;

        if (IsNavigationCompact)
            CollapseAllNavigationItems();
    }

    private void CollapseAllNavigationItems()
    {
        foreach (var item in NavigationItems)
            CollapseRecursive(item);
    }

    private static void CollapseRecursive(NavigationItem item)
    {
        item.IsExpanded = false;

        foreach (var child in item.Children)
            CollapseRecursive(child);
    }
}

    
