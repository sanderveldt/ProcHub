using System.Windows;
using System.Windows.Controls;
using ProcHub.Wpf.Features.PurchaseOrders.Dashboard;
using ProcHub.Wpf.Features.PurchaseOrders.Dashboard.ViewModels;

namespace ProcHub.Wpf.Features.PurchaseOrders.Dashboard.Views;

public partial class DashboardView : UserControl
{
    public DashboardView()
    {
        InitializeComponent();
    }

    private async void DashboardView_OnLoaded(
        object sender,
        RoutedEventArgs e)
    {
        if (DataContext is DashboardViewModel viewModel)
        {
            await viewModel.LoadAsync();
        }
    }
}