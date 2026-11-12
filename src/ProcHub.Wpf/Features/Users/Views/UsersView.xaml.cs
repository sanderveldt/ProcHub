using ProcHub.Wpf.Features.Users.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ProcHub.Wpf.Features.Users.Views;

public partial class UsersView : UserControl
{
    public UsersView()
    {
        InitializeComponent();
    }

    private async void Save_Click(
    object sender,
    RoutedEventArgs e)
    {
        if (DataContext is not UsersViewModel vm)
            return;

        try
        {
            await vm.SaveCommand.ExecuteAsync(
                NewPasswordBox.Password);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        finally
        {
            NewPasswordBox.Clear();
        }
    }
}
