using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcHub.Wpf.Infrastructure.Navigation;
using ProcHub.Wpf.Shell.Models;
using ProcHub.Wpf.Features.Settings.ViewModels;
using ProcHub.Wpf.Infrastructure.Authentication;
using ProcHub.Wpf.Features.Home.ViewModels;
using ProcHub.Wpf.Features.PaymentTerms.ViewModels;
using OpenOrdersDashboardViewModel =
    ProcHub.Wpf.Features.PurchaseOrders.Open.Dashboard.ViewModels.DashboardViewModel;
using ProcHub.Wpf.Features.ShippingTerms.ViewModels;
using ProcHub.Wpf.Features.Suppliers.ViewModels;
using ProcHub.Wpf.Features.Users.ViewModels;

namespace ProcHub.Wpf.Shell.ViewModels;

public sealed class MainWindowViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private bool _isNavigationCompact;

    public MainWindowViewModel(
        INavigationService navigationService,
        AuthSession authSession)
    {
        _navigationService = navigationService;
        Navigation = navigationService;

        if (!authSession.IsAuthenticated)
        {
            throw new InvalidOperationException(
                "You have to be authenticated to access ProcHub.");
        }

        var role = authSession.Role!;

        ToggleNavigationCommand = new RelayCommand(ToggleNavigation);
        SettingsCommand = NavigateCommand<SettingsViewModel>();

        var allNavItems = CreateNavigationItems();

        NavigationItems = new ObservableCollection<NavigationItem>(
            FilterNavigationbyRole(allNavItems, role));
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
            {
                return;
            }

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
        {
            item.Command.Execute(null);
        }
    }

    private IEnumerable<NavigationItem> CreateNavigationItems()
    {
        return 
        [
            new(
                "Home",
                "\uE80F",
                NavigateCommand<HomeViewModel>()),

            new(
                "Purchase Orders",
                "\xE70A",
                children:
                [
                    new(
                        "Dashboard",
                        "\xE7F4",
                        NavigateCommand<OpenOrdersDashboardViewModel>())
                ]),
            
            new(
                "Suppliers",
                "\uE716",
                NavigateCommand<SuppliersViewModel>()),
         
            new(
                "Master Data",
                "\uE8F1",
                children:
                [
                    new(
                        "Payment Terms",
                        "\uE8C7",
                        NavigateCommand<PaymentTermsViewModel>()),
                
                    new(
                        "Shipping Terms",
                        "\xE7E3",
                        NavigateCommand<ShippingTermsViewModel>())
                ]),
            
            new(
                "Administration",
                "\uE77B",
                allowedRoles:
                [
                    AppRoles.SuperUser,
                    AppRoles.Admin
                ],
                children:
                [
                    new(
                        "Users",
                        "\uE716",
                        NavigateCommand<UsersViewModel>())
                ])
        ]; 
    }

    private static IEnumerable<NavigationItem> FilterNavigationbyRole(
        IEnumerable<NavigationItem> items,
        string role)
    {
        foreach (var item in items)
        {
            if (!item.IsAllowedFor(role))
            {
                continue;
            }

            var visibleItems = FilterNavigationbyRole(
                item.Children,
                role)
            .ToArray();

            if (item.Command is null &&
                item.Children.Count > 0 &&
                visibleItems.Length == 0)
            {
                continue;
            }

            yield return new NavigationItem(
                item.Title,
                item.IconGlyph,
                item.Command,
                visibleItems,
                item.AllowedRoles);
        }
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
        {
            CollapseAllNavigationItems();
        }
    }

    private void CollapseAllNavigationItems()
    {
        foreach (var item in NavigationItems)
        {
            CollapseRecursive(item);
        }
    }

    private static void CollapseRecursive(NavigationItem item)
    {
        item.IsExpanded = false;

        foreach (var child in item.Children)
        {
            CollapseRecursive(child);
        }
    }
}