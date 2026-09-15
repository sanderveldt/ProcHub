using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProcHub.Wpf.Features.Home.ViewModels;
using ProcHub.Wpf.Features.PaymentTerms.ViewModels;
using ProcHub.Wpf.Features.Settings.ViewModels;
using ProcHub.Wpf.Features.ShippingTerms.ViewModels;
using ProcHub.Wpf.Features.Suppliers.ViewModels;
using ProcHub.Wpf.Features.Users.ViewModels;
using ProcHub.Wpf.Infrastructure.Navigation;
using ProcHub.Wpf.Shell.ViewModels;
using ProcHub.Wpf.Shell.Views;

using OpenOrdersDashboardViewModel = 
    ProcHub.Wpf.Features.PurchaseOrders.Open.Dashboard.ViewModels.DashboardViewModel;

namespace ProcHub.Wpf;
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        var builder = Host.CreateApplicationBuilder();
        ConfigureServices(builder.Services);

        _host = builder.Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _host.StartAsync()
             .GetAwaiter()
             .GetResult();

        var navigation = _host.Services.GetRequiredService<INavigationService>();
        navigation.NavigateTo<HomeViewModel>();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow = mainWindow;
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host.StopAsync()
             .GetAwaiter()
             .GetResult();

        _host.Dispose();

        base.OnExit(e);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<INavigationService, NavigationService>();

        // Shell
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<HomeViewModel>();
        services.AddSingleton<SettingsViewModel>();
    
        services.AddSingleton<OpenOrdersDashboardViewModel>();

        services.AddSingleton<SuppliersViewModel>();
        services.AddSingleton<PaymentTermsViewModel>();
        services.AddSingleton<ShippingTermsViewModel>();
        services.AddSingleton<UsersViewModel>();
    }
}

