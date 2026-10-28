using System.Collections.ObjectModel;
using System.Security;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcHub.Contracts.PaymentTerms.Responses;
using ProcHub.Contracts.ShippingTerms.Responses;
using ProcHub.Contracts.Suppliers.Enums;
using ProcHub.Contracts.Suppliers.Requests;
using ProcHub.Contracts.Suppliers.Responses;
using ProcHub.Wpf.Infrastructure.Api;
using ProcHub.Wpf.Infrastructure.Api.Clients;
using ProcHub.Wpf.Infrastructure.Navigation;

namespace ProcHub.Wpf.Features.Suppliers.ViewModels;

public sealed partial class SuppliersViewModel : PageViewModel
{
    private readonly SuppliersApiClient _suppliersApiClient;
    private readonly PurchaseOrdersApiClient _purchaseOrdersApiClient;
    private readonly PaymentTermsApiClient _paymentTermsApiClient;
    private readonly ShippingTermsApiClient _shippingTermsApiClient;
    public SuppliersViewModel(
        SuppliersApiClient suppliersApiClient,
        PurchaseOrdersApiClient purchaseOrdersApiClient,
        PaymentTermsApiClient paymentTermsApiClient,
        ShippingTermsApiClient shippingTermsApiClient)
        : base("Suppliers", "Maintain suppliers.")
    {
        _suppliersApiClient = suppliersApiClient;
        _purchaseOrdersApiClient = purchaseOrdersApiClient;
        _paymentTermsApiClient = paymentTermsApiClient;
        _shippingTermsApiClient = shippingTermsApiClient;
    }

    public ObservableCollection<SupplierListItemResponse> Suppliers
        { get; } = new();

    public ObservableCollection<PaymentTermListItemResponse> PaymentTerms
        { get; } = new();

    public ObservableCollection<ShippingTermResponse> ShippingTerms
        { get; } = new();

    public IReadOnlyList<SupplierStatus> SupplierStatuses
        { get; } = Enum.GetValues<SupplierStatus>();
    
    [ObservableProperty]
    public partial string Code { get; private set; } = string.Empty;

    // add Notify CanSave commands later
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;
    
    [ObservableProperty]
    public partial int? DefaultPaymentTermId { get; set; }

    [ObservableProperty]
    public partial string DefaultPaymentTermDescription { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string? FullName { get; set; } 

    [ObservableProperty]
    public partial int? MainShippingTermId { get; set; }

    [ObservableProperty]
    public partial string? MainShippingTermName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial int? SecondaryShippingTermId { get; set; }

    [ObservableProperty]
    public partial string? SecondaryShippingTermName { get; set; } 

    [ObservableProperty]
    public partial int? SampleShippingTermId { get; set; }

    [ObservableProperty]
    public partial string? SampleShippingTermName { get; set; }

    [ObservableProperty]
    public partial int? SecondaryPaymentTermId { get; set; }

    [ObservableProperty]
    public partial string? SecondaryPaymentTermDescription { get; set; }

    [ObservableProperty]
    public partial int? SamplePaymentTermId { get; set; }

    [ObservableProperty]
    public partial string? SamplePaymentTermDescription { get; set; }

    [ObservableProperty]
    public partial int? ShippingTimeDays { get; set; }

    [ObservableProperty]
    public partial int? ProductionTimeDays { get; set; }

    [ObservableProperty]
    public partial int? MainLeadTimeDays { get; set; }

    [ObservableProperty]
    public partial int? SecondaryLeadTimeDays { get; set; }

    [ObservableProperty]
    public partial int? SampleLeadTimeDays { get; set; }

    [ObservableProperty]
    public partial SupplierStatus? Status { get; set; } 

    [ObservableProperty]
    public partial DateOnly? CreationDate { get; private set; }

    [ObservableProperty]
    public partial string? ResponsibleBuyerName { get; private set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanEditFields))]
    public partial bool IsCreating { get; private set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEditorReadOnly))]
    [NotifyPropertyChangedFor(nameof(CanEditFields))]
    public partial bool IsEditing { get; private set; }
    public bool IsEditorReadOnly => !IsEditing;
    public bool CanEditFields => IsCreating || IsEditing;

    [ObservableProperty]
    public partial SupplierListItemResponse? SelectedSupplier { get; set; }

    async partial void OnSelectedSupplierChanged(
        SupplierListItemResponse? value)
    {
        if (value is null)
        {
            return;
        }

        await LoadSupplierAsync(value.Code);

        IsCreating = false;
        IsEditing = false; 
    }


    private bool FieldsCheck()
    {
        return !string.IsNullOrWhiteSpace(Name)
            && DefaultPaymentTermId is >= 0
            && MainShippingTermId is >= 0;
    }

    [RelayCommand]
    private void OnNew()
    {
        SelectedSupplier = null;

        Code = string.Empty;
        Name = string.Empty;
        DefaultPaymentTermId = null;
        DefaultPaymentTermDescription = string.Empty;
        FullName = string.Empty;
        MainShippingTermId = null;
        MainShippingTermName = string.Empty;
        SecondaryShippingTermId = null;
        SecondaryShippingTermName = string.Empty;
        SampleShippingTermId = null;
        SampleShippingTermName = string.Empty;
        SecondaryPaymentTermId = null;
        SecondaryPaymentTermDescription = string.Empty;
        SamplePaymentTermId = null;
        SamplePaymentTermDescription = string.Empty;
        ShippingTimeDays = null;
        ProductionTimeDays = null;
        MainLeadTimeDays = null;
        SecondaryLeadTimeDays = null;
        SampleLeadTimeDays = null;
        Status = null;
        CreationDate = null;
        ResponsibleBuyerName = string.Empty;

        ClearError();
    }



    public async Task LoadAllAsync(
        CancellationToken cancellationToken = default)
    {
        ClearError();

        await Task.WhenAll(
            LoadAllSuppliersAsync(cancellationToken),
            LoadPaymentTermsAsync(cancellationToken),
            LoadShippingTermsAsync(cancellationToken));
    }

    public async Task LoadAllSuppliersAsync(
        CancellationToken cancellationToken = default)
    {
        ClearError();

        try
        {
            var suppliers = await _suppliersApiClient
                .GetAllAsync(cancellationToken);
            
            Suppliers.Clear();

            foreach (var supplier in suppliers)
            {
                Suppliers.Add(supplier);
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
    }

    public async Task LoadSupplierAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        ClearError();

        try
        {
            var supplier = await _suppliersApiClient
                .GetByCodeAsync(
                    code,
                    cancellationToken);
            
            Code = supplier.Code;
            Name = supplier.Name;

            DefaultPaymentTermId = 
                supplier.DefaultPaymentTermId;
            DefaultPaymentTermDescription =
                supplier.DefaultPaymentTermDescription;
            
            FullName = supplier.FullName;

            MainShippingTermId = 
                supplier.MainShippingTermId;
            MainShippingTermName = 
                supplier.MainShippingTermName;
            
            SecondaryShippingTermId = 
                supplier.SecondaryShippingTermId;
            SecondaryShippingTermName =
                supplier.SecondaryShippingTermName;
            
            SampleShippingTermId = 
                supplier.SampleShippingTermId;
            SampleShippingTermName =
                supplier.SampleShippingTermName;

            SecondaryPaymentTermId = 
                supplier.SecondaryPaymentTermId;
            SecondaryPaymentTermDescription =
                supplier.SecondaryPaymentTermDescription;
            
            ShippingTimeDays = supplier.ShippingTimeDays;
            ProductionTimeDays = supplier.ProductionTimeDays;
            MainLeadTimeDays = supplier.MainLeadTimeDays;
            SecondaryLeadTimeDays = supplier.SecondaryLeadTimeDays;
            SampleLeadTimeDays = supplier.SampleLeadTimeDays;

            Status = supplier.Status;
            CreationDate = supplier.CreationDate;
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

    private async Task CreateAsync(
        CancellationToken cancellationToken = default)
    {
        var request = new 
            CreateSupplierRequest(
                Code,
                Name,
                DefaultPaymentTermId,
                MainShippingTermId)
        {
            FullName = FullName,
            SecondaryShippingTermId = SecondaryShippingTermId,
            SampleShippingTermId = SampleShippingTermId,
            ShippingTimeDays = ShippingTimeDays,
            ProductionTimeDays = ProductionTimeDays,
            SecondaryPaymentTermId = SecondaryPaymentTermId,
            MainLeadTimeDays = MainLeadTimeDays,
            SecondaryLeadTimeDays = SecondaryLeadTimeDays,
            SampleLeadTimeDays = SampleLeadTimeDays
        };

        var newSupplier = await _suppliersApiClient
            .CreateAsync(
                request,
                cancellationToken);
        
        // Add newSupplier transfer to listoverview later
    }
    public async Task LoadPaymentTermsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var paymentTerms = await _paymentTermsApiClient
                .GetAllAsync(cancellationToken);
            
            PaymentTerms.Clear();

            foreach (var paymentTerm in paymentTerms)
            {
                PaymentTerms.Add(paymentTerm);
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
    }

    public async Task LoadShippingTermsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var shippingTerms = await _shippingTermsApiClient
                .GetAllAsync(cancellationToken);
        
            ShippingTerms.Clear();

            foreach (var shippingTerm in shippingTerms)
            {
                ShippingTerms.Add(shippingTerm);
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
    }

}
