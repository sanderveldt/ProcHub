using System.Collections.ObjectModel;
using System.Security;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcHub.Contracts.Suppliers.Enums;
using ProcHub.Contracts.Suppliers.Requests;
using ProcHub.Contracts.Suppliers.Responses;
using ProcHub.Wpf.Infrastructure.Api;
using ProcHub.Wpf.Infrastructure.Api.Clients;
using ProcHub.Wpf.Infrastructure.Navigation;

namespace ProcHub.Wpf.Features.Suppliers.ViewModels;

public sealed partial class SuppliersViewModel : PageViewModel
{
    private readonly SuppliersApiClient _apiClient;
    public SuppliersViewModel(
        SuppliersApiClient apiClient)
        : base("Suppliers", "Maintain suppliers.")
    {
        _apiClient = apiClient;
    }

    public ObservableCollection<SupplierListItemResponse> Suppliers
        { get; } = new();
    

    [ObservableProperty]
    public partial string Code { get; private set; } = string.Empty;

    // add Notify CanSave commands later
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;
    
    [ObservableProperty]
    public partial int DefaultPaymentTermId { get; set; }

    [ObservableProperty]
    public partial string DefaultPaymentTermDescription { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string? FullName { get; set; } 

    [ObservableProperty]
    public partial int MainShippingTermId { get; set; }

    [ObservableProperty]
    public partial string MainShippingTermName { get; set; } = string.Empty;

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
    public partial int? ShippingTimeDays { get; set; }

    [ObservableProperty]
    public partial int? ProductionTimeDays { get; set; }

    [ObservableProperty]
    public partial int MainLeadTimeDays { get; set; }

    [ObservableProperty]
    public partial int? SecondaryLeadTimeDays { get; set; }

    [ObservableProperty]
    public partial int? SampleLeadTimeDays { get; set; }

    [ObservableProperty]
    public partial string Status { get; set; } = string.Empty;

    [ObservableProperty]
    public partial DateOnly? CreationDate { get; private set; }

    [ObservableProperty]
    public partial string? ResponsibleBuyerName { get; private set; }

    [ObservableProperty]
    public partial bool IsCreating { get; private set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEditorReadOnly))]
    public partial bool IsEditing { get; private set; }
    public bool IsEditorReadOnly => !IsEditing;

    [ObservableProperty]
    public partial SupplierListItemResponse? SelectedSupplier { get; set; }

    async partial void OnSelectedSupplierChanged(
        SupplierListItemResponse? value)
    {
        if (value is null)
        {
            return;
        }

        await LoadAsync(value.Code);

        IsCreating = false;
        IsEditing = false; 
    }


    private bool FieldsCheck()
    {
        return !string.IsNullOrWhiteSpace(Name)
            && DefaultPaymentTermId is >= 0
            && MainShippingTermId is >= 0;
    }







    public async Task LoadAllAsync(
        CancellationToken cancellationToken = default)
    {
        ClearError();

        try
        {
            var suppliers = await _apiClient
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

    public async Task LoadAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        ClearError();

        try
        {
            var supplier = await _apiClient
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

}
