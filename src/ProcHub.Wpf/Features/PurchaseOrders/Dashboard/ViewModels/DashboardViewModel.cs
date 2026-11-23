using System.Collections.ObjectModel;
using System.Security;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcHub.Contracts.PurchaseOrders.Enums;
using ProcHub.Contracts.PurchaseOrders.Requests;
using ProcHub.Contracts.PurchaseOrders.Responses;
using ProcHub.Contracts.Suppliers.Responses;
using ProcHub.Wpf.Infrastructure.Api;
using ProcHub.Wpf.Infrastructure.Api.Clients;
using ProcHub.Wpf.Infrastructure.Authentication;
using ProcHub.Wpf.Infrastructure.Navigation;

namespace ProcHub.Wpf.Features.PurchaseOrders.Dashboard.ViewModels;

public sealed partial class DashboardViewModel : PageViewModel
{
    private readonly PurchaseOrdersApiClient _orderApiClient;
    private readonly SuppliersApiClient _suppliersApiClient;
    private readonly AuthSession _authSession;
    private bool _suppressSave;
    
    public DashboardViewModel(
        PurchaseOrdersApiClient OrderApiClient,
        SuppliersApiClient suppliersApiClient,
        AuthSession authSession)
        : base(
            "Open Purchase Orders",
            "Open Purchase Orders dashboard overview.")
    {
        _orderApiClient = OrderApiClient;
        _suppliersApiClient = suppliersApiClient;
        _authSession = authSession;
    }

    public bool AllOrdersLoaded = false;

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

            var userId = _authSession.CurrentUser?.Id
                ?? throw new InvalidOperationException(
                    "No user logged in.");

            var orders = await _orderApiClient
                .GetDashboardListAsync(
                    userId,
                    cancellationToken);
            
            SelectedPurchaseOrder = null;

            PurchaseOrders.Clear();

            foreach (var order in orders)
            {
                PurchaseOrders.Add(
                    new DashboardRowViewModel(
                        order,
                        _orderApiClient));
            }

            AllOrdersLoaded = false;
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
    public async Task LoadAllOrdersAsync(
        CancellationToken cancellationToken)
    {
        ClearError();

        await SavePendingChangesAsync(cancellationToken);

        try
        {
            IsLoading = true;
            _suppressSave = true;

            var orders = await _orderApiClient
                .GetDashboardListAsync(
                    null,
                    cancellationToken);
            
            SelectedPurchaseOrder = null;

            PurchaseOrders.Clear();

            foreach (var order in orders)
            {
                PurchaseOrders.Add(
                    new DashboardRowViewModel(
                        order,
                        _orderApiClient));
            }       

            AllOrdersLoaded = true;
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
        ClearError();

        try
        {
            await SavePendingChangesAsync(cancellationToken);

            if (AllOrdersLoaded)
            {
                await LoadAllOrdersAsync(cancellationToken);
            }
            else
            {
                await LoadAsync(cancellationToken);
            }
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            ShowError($"Unable to save changes: {ex.Message}");
        }
    }

    public async Task SavePendingChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var dirtyRows = PurchaseOrders
            .Where(x => x.IsDirty || x.IsSaving)
            .ToArray();
        
        foreach (var row in dirtyRows)
        {
            cancellationToken
                .ThrowIfCancellationRequested();

            await row.SaveChangesAsync(
                cancellationToken);
        }
    }

    public IReadOnlyList<PurchaseOrderType> OrderTypes
        { get; } = Enum.GetValues<PurchaseOrderType>();

    public ObservableCollection<SupplierListItemResponse> Suppliers { get; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial bool IsCreating { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial string Code { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial PurchaseOrderType? OrderType { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial SupplierListItemResponse? SelectedSupplier { get; set; }

    [RelayCommand]
    private async Task NewAsync(CancellationToken cancellationToken)
    {
        ClearError();

        try
        {
            var suppliers = await _suppliersApiClient.GetAllAsync(cancellationToken);

            Suppliers.Clear();
            foreach (var supplier in suppliers)
            {
                Suppliers.Add(supplier);
            }

            Code = string.Empty;
            OrderType = null;
            SelectedSupplier = null;
            IsCreating = true;
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (ApiException ex)
        {
            ShowError(ex.Message);
        }
    }

    private bool FieldsCheck()
    {
        return IsCreating
            && !string.IsNullOrWhiteSpace(Code)
            && OrderType is PurchaseOrderType orderType
            && Enum.IsDefined(orderType)
            && SelectedSupplier is { Id: > 0 };
    }

    [RelayCommand(CanExecute = nameof(FieldsCheck))]
    private async Task SaveAsync(
        CancellationToken cancellationToken)
    {
        ClearError();

        try
        {
            await CreateAsync(cancellationToken);
            IsCreating = false;
            await RefreshAsync(cancellationToken);
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (ApiException ex)
        {
            ShowError(ex.Message);
        }
    }

    public async Task CreateAsync(
        CancellationToken cancellationToken = default)
    {
        var request = new CreatePurchaseOrderRequest(
            Code.Trim(),
            OrderType!.Value,
            SelectedSupplier!.Id);

        await _orderApiClient
            .CreateAsync(
                request,
                cancellationToken);
    }
}
