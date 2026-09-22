using ProcHub.Wpf.Features.PaymentTerms.ViewModels;
using ProcHub.Wpf.Features.ShippingTerms.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ProcHub.Wpf.Features.PaymentTerms.Views;
public partial class PaymentTermsView : UserControl
{
    public PaymentTermsView()
    {
        InitializeComponent();
    }

    private async void PaymentTermView_OnLoaded(
        object sender,
        RoutedEventArgs e)
    {
        if (DataContext is PaymentTermsViewModel viewModel)
        {
            await viewModel.LoadAllAsync();
        }
    }
}

