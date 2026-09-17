using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProcHub.Contracts.Authentication.Responses;
using ProcHub.Wpf.Features.Home.ViewModels;
using ProcHub.Wpf.Features.PaymentTerms.ViewModels;
using ProcHub.Wpf.Features.Settings.ViewModels;
using ProcHub.Wpf.Features.ShippingTerms.ViewModels;
using ProcHub.Wpf.Features.Suppliers.ViewModels;
using ProcHub.Wpf.Features.Users.ViewModels;
using ProcHub.Wpf.Infrastructure.Api;
using ProcHub.Wpf.Infrastructure.Api.Clients;
using ProcHub.Wpf.Infrastructure.Authentication;
using ProcHub.Wpf.Infrastructure.Navigation;
using ProcHub.Wpf.Shell.ViewModels;
using ProcHub.Wpf.Shell.Views;
using ProcHub.Wpf.Infrastructure.Services;

using OpenOrdersDashboardViewModel = 
    ProcHub.Wpf.Features.PurchaseOrders.Open.Dashboard.ViewModels.DashboardViewModel;
using System.Net.Sockets;

namespace ProcHub.Wpf;
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        var builder = Host.CreateApplicationBuilder(
            new HostApplicationBuilderSettings
            {
                ContentRootPath = AppContext.BaseDirectory
            });
        
        ConfigureServices(
            builder.Services,
            builder.Configuration);

        _host = builder.Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try{

        
            await _host.StartAsync();

        
            // Temporary Development authlogin, remove later
            var authentication = _host
                .Services.GetRequiredService<AuthenticationService>();
        
            const string devEmail = "admin@ProcHub.local";
            const string devPassword = "StrongPassword123!";

            await authentication.LoginAsync(
                    devEmail,
                    devPassword);
            

            var navigation = _host
                .Services.GetRequiredService<INavigationService>();

            navigation.NavigateTo<HomeViewModel>();

            var mainWindow = _host
                .Services.GetRequiredService<MainWindow>();

            MainWindow = mainWindow;

            mainWindow.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.ToString(),
                "start up failed",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            
            Shutdown();
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host.StopAsync()
             .GetAwaiter()
             .GetResult();

        _host.Dispose();

        base.OnExit(e);
    }

    private static void ConfigureServices(
        IServiceCollection services,
        IConfiguration configuration)
    {
        var apiBaseUrl = configuration
            ["Api:BaseUrl"]
            ?? throw new InvalidOperationException(
                "Api:BaseUrl is not configured.");
        
        if (!Uri.TryCreate(
            apiBaseUrl,
            UriKind.Absolute,
            out var apiBaseUri))
        {
            throw new InvalidOperationException(
                $"Api:BaseUrl '{apiBaseUrl}'is invalid.");
        }

        services.AddSingleton<AuthSession>();
        services.AddSingleton<TokenStore>();

        services.AddSingleton<AuthApiClient>();
        services.AddTransient<BearerTokenHandler>();

        services.AddSingleton<ShippingTermsApiClient>();

        services.AddHttpClient(
            ApiClientNames.Authentication,
            client =>
            {
                ConfigureApiClient(
                    client,
                    apiBaseUri);
                
            });

        services.AddHttpClient(
            ApiClientNames.Authorized,
            client =>
            {
                ConfigureApiClient(
                    client,
                    apiBaseUri);
            })
            .AddHttpMessageHandler<BearerTokenHandler>();
            

        services.AddSingleton<AuthenticationService>();
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

    private static void ConfigureApiClient(
        HttpClient client,
        Uri apiBaseUri)
    {
        client.BaseAddress = apiBaseUri;

        client.Timeout = TimeSpan.FromSeconds(30);

        client.DefaultRequestHeaders
            .Accept.Clear();

        client.DefaultRequestHeaders
            .Accept.Add(
                new MediaTypeWithQualityHeaderValue(
                    "application/json"));
    }
}