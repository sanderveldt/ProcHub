using System.Windows;
using ProcHub.Wpf.Features.Authentication.ViewModels;

namespace ProcHub.Wpf.Features.Authentication.Views;

public partial class LoginWindow : Window
{
    public LoginWindow(LoginViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;

        viewModel.LoginSucceeded += OnLoginSucceeded;
    }

    private async void OnLoginClick(object sender, RoutedEventArgs e)
    {
        var command = ((LoginViewModel)DataContext).LoginCommand;
        var password = PasswordInput.Password;
        if (command.CanExecute(password))
        {
            await command.ExecuteAsync(password);
        }
    }

    private void OnLoginSucceeded(
        object? sender,
        EventArgs e)
    {
        
        DialogResult = true;
    }
}
