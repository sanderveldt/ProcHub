using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcHub.Wpf.Infrastructure.Api;
using ProcHub.Wpf.Infrastructure.Services;

namespace ProcHub.Wpf.Features.Authentication.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthenticationService _authentication;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private bool _isBusy;

    public event EventHandler? LoginSucceeded;

    public LoginViewModel(AuthenticationService authentication)
    {
        _authentication = authentication;
    }

    private bool CanLogin() => !IsBusy;

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync(string? password)
    {
        IsBusy = true;
        ErrorMessage = string.Empty;

        try
        {
            await _authentication.LoginAsync(
                Email.Trim(),
                password ?? string.Empty);

            if (!_authentication.IsAuthenticated)
            {
                throw new InvalidOperationException(
                    "Authentication was not completed.");
            }

            LoginSucceeded?.Invoke(this, EventArgs.Empty);
        }
        catch (ApiException)
        {
            ErrorMessage =
                "Login failed. Check your credentials or API connection.";
        }
        catch (Exception)
        {
            ErrorMessage = "An unexpected error occurred during login.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
