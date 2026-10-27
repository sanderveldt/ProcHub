using System.Windows;
using System.Windows.Controls;
using ProcHub.Wpf.Features.Suppliers.ViewModels;

namespace ProcHub.Wpf.Features.Suppliers.Views;

public partial class SuppliersView : UserControl
{
    public SuppliersView()
    {
        InitializeComponent();
    }

    private async void SuppliersView_OnLoaded(
        object sender,
        RoutedEventArgs e)
    {
        if (DataContext is SuppliersViewModel viewModel)
        {
            await viewModel.LoadAllAsync();
        }
    }
}