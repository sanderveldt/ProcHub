using System.Windows;
using System.Windows.Controls;
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

    private void OnLoginSucceeded(
        object? sender,
        EventArgs e)
    {
        
        DialogResult = true;
    }
}