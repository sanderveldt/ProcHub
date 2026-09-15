using ProcHub.Wpf.Infrastructure.Navigation;

namespace ProcHub.Wpf.Features.PurchaseOrders.Open.Dashboard.ViewModels;

public sealed class DashboardViewModel : PageViewModel
{
    public DashboardViewModel()
        : base(
            "Open Purchase Orders",
            "Open Purchase Orders dashboard overview.")
    {
    }
}