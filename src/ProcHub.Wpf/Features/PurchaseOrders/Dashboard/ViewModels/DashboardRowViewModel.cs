using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcHub.Contracts.PurchaseOrders.Enums;
using ProcHub.Contracts.PurchaseOrders.Requests;
using ProcHub.Contracts.PurchaseOrders.Responses;
using ProcHub.Wpf.Infrastructure.Api;
using ProcHub.Wpf.Infrastructure.Api.Clients;
using ProcHub.Wpf.Features.PurchaseOrders.Dashboard;

namespace ProcHub.Wpf.Features.PurchaseOrders.Dashboard.ViewModels;

public sealed partial class DashboardRowViewModel : ObservableObject
{
    private readonly PurchaseOrdersApiClient _apiClient;
    private bool _suppressChanges;

    public DashboardRowViewModel(
        PurchaseOrderResponse order,
        PurchaseOrdersApiClient apiClient)
    {
        _apiClient = apiClient;
        _suppressChanges = true;

        Code = order.Code;
        OrderDate = order.OrderDate;
        OrderType = order.OrderType;
        OrderStatus = order.OrderStatus;

        SupplierId = order.SupplierId;
        SupplierCode = order.SupplierCode;
        SupplierName = order.SupplierName;

        PaymentTermId = order.PaymentTermId;
        PaymentTermDescription = order.PaymentTermDescription;

        BalanceDueDays = order.BalanceDueDays;
        BalanceDueDate = order.BalanceDueDate;

        DepositPercentage = order.DepositPercentage;

        OrderAmount = order.OrderAmount;
        DepositAmount = order.DepositAmount;
        BalanceAmount = order.BalanceAmount;
        
        DepositPaid = order.DepositPaid;
        DepositPaidDate = order.DepositPaidDate;

        OrderPaid = order.OrderPaid;
        BalancePaidDate = order.BalancePaidDate;

        ShippingTermId = order.ShippingTermId;
        ShippingTermName = order.ShippingTermName;

        ConfirmationReference = order.ConfirmationReference;
        InvoiceReference = order.InvoiceReference;
        OrderNotes = order.OrderNotes;
        ShipmentSize = order.ShipmentSize;
        ShipmentReference = order.ShipmentReference;

        ConfirmationDate = order.ConfirmationDate;
        RequestedProductionDate = order.RequestedProductionDate;
        ProductionDoneDate = order.ProductionDoneDate;
        InvoiceDate = order.InvoiceDate;
        ShippingDate = order.ShippingDate;
        BillOfLadingDate = order.BillOfLadingDate;
        ArrivalDate = order.ArrivalDate;
        ExpectedDeliveryDate = order.ExpectedDeliveryDate;
        DeliveryDate = order.DeliveryDate;

        ProductionTimeDays = order.ProductionTimeDays;
        ShippingTimeDays = order.ShippingTimeDays;
        LeadTimeDays = order.LeadTimeDays;

        ConfirmationSent = order.ConfirmationSent;
        InvoiceSent = order.InvoiceSent;
        InspectionDone = order.InspectionDone;
        CertificateOfOriginSaved = order.CertificateOfOriginSaved;

        _suppressChanges = false;
    }

    // Read only fields, Add supplier update later maybe?
    public string Code { get; }
    public DateOnly OrderDate { get; }
    public PurchaseOrderType OrderType { get; }

    // Add Orderstatus update later
    public PurchaseOrderStatus OrderStatus { get; }
    
    public int SupplierId { get; }
    public string SupplierCode { get; }
    public string SupplierName { get; }

    public int PaymentTermId { get; }
    public string PaymentTermDescription { get; }
    public int BalanceDueDays { get; }
    public DateOnly? BalanceDueDate { get; }
    public decimal DepositPercentage { get; }

    [ObservableProperty]
    public partial decimal OrderAmount { get; set; }

    [ObservableProperty]
    public partial bool DepositPaid { get; set; }

    [ObservableProperty]
    public partial DateOnly? DepositPaidDate { get; set; }

    [ObservableProperty]
    public partial bool OrderPaid { get; set; }

    [ObservableProperty]
    public partial DateOnly? BalancePaidDate { get; set; }

    // Calculated fields
    public decimal DepositAmount { get; }
    public decimal BalanceAmount { get; }

    public int ShippingTermId { get; }
    public string ShippingTermName { get; }

    [ObservableProperty]
    public partial string? ConfirmationReference { get; set; }

    [ObservableProperty]
    public partial string? InvoiceReference { get; set; }

    [ObservableProperty]
    public partial string? OrderNotes { get; set; }

    [ObservableProperty]
    public partial string? ShipmentSize { get; set; }

    [ObservableProperty]
    public partial string? ShipmentReference { get; set; }


    [ObservableProperty]
    public partial DateOnly? ConfirmationDate { get; set; }

    [ObservableProperty]
    public partial DateOnly? RequestedProductionDate { get; set; }

    [ObservableProperty]
    public partial DateOnly? ProductionDoneDate { get; set; }

    [ObservableProperty]
    public partial DateOnly? InvoiceDate { get; set; }

    [ObservableProperty]
    public partial DateOnly? ShippingDate { get; set; }

    [ObservableProperty]
    public partial DateOnly? BillOfLadingDate { get; set; }

    [ObservableProperty]
    public partial DateOnly? ArrivalDate { get; set; }

    [ObservableProperty]
    public partial DateOnly? ExpectedDeliveryDate { get; set; }

    [ObservableProperty]
    public partial DateOnly? DeliveryDate { get; set; }


    // Calculated fields
    public int? ProductionTimeDays { get; }
    public int? ShippingTimeDays { get; }
    public int? LeadTimeDays { get; }

    // Status flags, add dirty markings and update api call later
    [ObservableProperty]
    public partial bool ConfirmationSent { get; set; }

    [ObservableProperty]
    public partial bool InvoiceSent { get; set; }

    [ObservableProperty]
    public partial bool InspectionDone { get; set; }

    [ObservableProperty]
    public partial bool CertificateOfOriginSaved { get; set; }


    // UI states
    [ObservableProperty]
    public partial bool IsExpanded { get; set; }

    [ObservableProperty]
    public partial DashboardDetailsSection 
        ActiveDetailsSection { get; set; }

    [RelayCommand]
    private void ToggleDetailsSection(
        DashboardDetailsSection section)
    {
        if (section == DashboardDetailsSection.None ||
            (IsExpanded && ActiveDetailsSection == section))
        {
            ActiveDetailsSection = DashboardDetailsSection.None;
            IsExpanded = false;
            return;
        }

        ActiveDetailsSection = section;
        IsExpanded = true;
    }

    [ObservableProperty]
    public partial bool IsSaving { get; private set; }

    [ObservableProperty]
    public partial string? SaveError { get; private set; }

    // Dirty states, add other updates dirty states later
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsDirty))]
    public partial bool IsDatesDirty { get; private set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsDirty))]
    public partial bool IsReferencesDirty { get; private set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsDirty))]
    public partial bool IsPaymentsDirty { get; private set; }

    public bool IsDirty => 
        IsDatesDirty || 
        IsReferencesDirty || 
        IsPaymentsDirty;

    // DATE CHANGES
    partial void OnConfirmationDateChanged(DateOnly? value)
    {
        MarkDatesDirty();
    } 

    partial void OnRequestedProductionDateChanged(DateOnly? value)
    {
        MarkDatesDirty();
    }

    partial void OnProductionDoneDateChanged(DateOnly? value)
    {
        MarkDatesDirty();
    }

    partial void OnInvoiceDateChanged(DateOnly? value)
    {
        MarkDatesDirty();
    }

    partial void OnShippingDateChanged(DateOnly? value)
    {
        MarkDatesDirty();
    }

    partial void OnBillOfLadingDateChanged(DateOnly? value)
    {
        MarkDatesDirty();
    } 

    partial void OnArrivalDateChanged(DateOnly? value)
    {
        MarkDatesDirty();
    }

    partial void OnExpectedDeliveryDateChanged(DateOnly? value)
    {
        MarkDatesDirty();
    }

    partial void OnDeliveryDateChanged(DateOnly? value)
    {
        MarkDatesDirty();
    }

    // REFERENCES AND NOTES CHANGES
    partial void OnConfirmationReferenceChanged(string? value)
    {
        MarkReferencesDirty();
    }

    partial void OnInvoiceReferenceChanged(string? value)
    {
        MarkReferencesDirty();
    }

    partial void OnOrderNotesChanged(string? value)
    {
        MarkReferencesDirty();
    }

    partial void OnShipmentSizeChanged(string? value)
    {
        MarkReferencesDirty();
    }

    partial void OnShipmentReferenceChanged(string? value)
    {
        MarkReferencesDirty();
    }

    // PAYMENTS CHANGES
    partial void OnOrderAmountChanged(decimal value)
    {
        MarkPaymentsDirty();
    }

    partial void OnDepositPaidChanged(bool value)
    {
        MarkPaymentsDirty();
    }

    partial void OnDepositPaidDateChanged(DateOnly? value)
    {
        MarkPaymentsDirty();
    }

    partial void OnOrderPaidChanged(bool value)
    {
        MarkPaymentsDirty();
    }

    partial void OnBalancePaidDateChanged(DateOnly? value)
    {
        MarkPaymentsDirty();
    }

    // MARK DIRTY STATES
    private void MarkDatesDirty()
    {
        if (_suppressChanges) 
        {
            return;
        }

        IsDatesDirty = true;
        SaveError = null;
    }

    private void MarkReferencesDirty()
    {
        if (_suppressChanges) 
        {
            return;
        }

        IsReferencesDirty = true;
        SaveError = null;
    }

    private void MarkPaymentsDirty()
    {
        if (_suppressChanges) 
        {
            return;
        }

        IsPaymentsDirty = true;
        SaveError = null;
    }

    // SAVE CHANGES
    // Saves when user moves from the row
    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        if (!IsDirty || IsSaving)
        {
            return;
        }

        try
        {
            IsSaving = true;
            SaveError = null;

            if (IsDatesDirty)
            {
                await SaveDatesAsync(
                    cancellationToken);
            }

            if (IsReferencesDirty)
            {
                await SaveReferencesAsync(
                    cancellationToken);
            }

            if (IsPaymentsDirty)
            {
                await SavePaymentsAsync(
                    cancellationToken);
            }
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (ApiException ex)
        {
            SaveError = ex.Message;
        }
        finally
        {
            IsSaving = false;
        }
    }

    // API CALLS
    private async Task SaveDatesAsync(
        CancellationToken cancellationToken)
    {
        var request = 
            new UpdatePurchaseOrderDatesRequest
            {
                OrderConfirmationDate = ConfirmationDate,
                RequestedProductionDate = RequestedProductionDate,
                ProductionDoneDate = ProductionDoneDate,
                InvoiceDate = InvoiceDate,
                ShippingDate = ShippingDate,
                BillOfLadingDate = BillOfLadingDate,
                ArrivalDate = ArrivalDate,
                ExpectedDeliveryDate = ExpectedDeliveryDate,
                DeliveryDate = DeliveryDate
            };

        await _apiClient.UpdateDatesAsync(
            Code,
            request,
            cancellationToken);

        IsDatesDirty = false;
    }

    private async Task SaveReferencesAsync(
        CancellationToken cancellationToken)
    {
        var request =
            new UpdatePurchaseOrderReferencesRequest
            {
                OrderConfirmationReference = ConfirmationReference,
                InvoiceReference = InvoiceReference,
                OrderNotes = OrderNotes,
                ShipmentSize = ShipmentSize,
                ShipmentReference = ShipmentReference
            };
        
        await _apiClient.UpdateReferencesAsync(
            Code,
            request,
            cancellationToken);

        IsReferencesDirty = false;
    }

    private async Task SavePaymentsAsync(
        CancellationToken cancellationToken)
    {
        var request =
            new UpdatePurchaseOrderPaymentRequest
            {
                OrderAmount = OrderAmount,
                DepositPaid = DepositPaid,
                DepositPaidDate = DepositPaidDate,
                OrderPaid = OrderPaid,
                BalancePaidDate = BalancePaidDate
            };

        await _apiClient.UpdatePaymentsAsync(
            Code,
            request,
            cancellationToken);
        
        IsPaymentsDirty = false;
    }

}