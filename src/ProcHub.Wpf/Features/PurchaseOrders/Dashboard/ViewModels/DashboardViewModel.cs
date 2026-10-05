using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcHub.Contracts.PurchaseOrders.Enums;
using ProcHub.Contracts.PurchaseOrders.Requests;
using ProcHub.Contracts.PurchaseOrders.Responses;
using ProcHub.Wpf.Infrastructure.Api;
using ProcHub.Wpf.Infrastructure.Api.Clients;
using ProcHub.Wpf.Infrastructure.Navigation;

namespace ProcHub.Wpf.Features.PurchaseOrders.Dashboard.ViewModels;

public sealed partial class DashboardViewModel : PageViewModel
{
    private readonly PurchaseOrdersApiClient _apiClient;
    private bool _suppressSave;
    
    public DashboardViewModel(
        PurchaseOrdersApiClient apiClient)
        : base(
            "Open Purchase Orders",
            "Open Purchase Orders dashboard overview.")
    {
        _apiClient = apiClient;
    }

    public ObservableCollection<DashboardRowViewModel>
        PurchaseOrders { get; } = [];
    
    [ObservableProperty]
    public partial bool IsLoading { get; private set; }

    [ObservableProperty]
    public partial DashboardRowViewModel? 
        SelectedPurchaseOrder { get; set; }

    partial void OnSelectedPurchaseOrderChanged(
        DashboardRowViewModel? oldValue, 
        DashboardRowViewModel? newValue)
    {
        if (_suppressSave)
        {
            return;
        }

        if (oldValue is null)
        {
            return;
        }

        if (ReferenceEquals(oldValue, newValue))
        {
            return;
        }

        if (!oldValue.IsDirty)
        {
            return;
        }

        _ = SaveRowAsync(oldValue);
    }

    private async Task SaveRowAsync(
        DashboardRowViewModel row)
    {
        try
        {
            await row.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            ShowError(
                $"Unable to save PO {row.Code}:{ex.Message}");
        }
    }

    public async Task LoadAsync(
        CancellationToken cancellationToken = default)
    {
        ClearError();

        try
        {
            IsLoading = true;
            _suppressSave = true;

            var orders = await _apiClient
                .GetDashboardListAsync(
                    cancellationToken);
            
            SelectedPurchaseOrder = null;

            PurchaseOrders.Clear();

            foreach (var order in orders)
            {
                PurchaseOrders.Add(
                    new DashboardRowViewModel(
                        order,
                        _apiClient));
            }
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (ApiException ex)
        {
            ShowError(ex.Message);
        }
        finally
        {
            _suppressSave = false;
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync(
        CancellationToken cancellationToken)
    {
        await SavePendingChangesAsync(
            cancellationToken);
        
        await LoadAsync(cancellationToken);
    }

    public async Task SavePendingChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var dirtyRows = PurchaseOrders
            .Where(x => x.IsDirty)
            .ToArray();
        
        foreach (var row in dirtyRows)
        {
            cancellationToken
                .ThrowIfCancellationRequested();

            await row.SaveChangesAsync(
                cancellationToken);
        }
    }
}