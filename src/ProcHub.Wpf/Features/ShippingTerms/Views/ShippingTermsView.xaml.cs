using ProcHub.Wpf.Features.ShippingTerms.ViewModels;
using System.Windows.Controls;
using System.Windows;

namespace ProcHub.Wpf.Features.ShippingTerms.Views;
public partial class ShippingTermsView : UserControl
{
    public ShippingTermsView()
    {
        InitializeComponent();
    }

    private async void ShippingTermsView_OnLoaded(
        object sender,
        RoutedEventArgs e)
    {
        if (DataContext is ShippingTermsViewModel viewModel)
        {
            await viewModel.LoadAllAsync();
        } 
    }   
}

