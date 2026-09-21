using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcHub.Contracts.PaymentTerms.Requests;
using ProcHub.Contracts.PaymentTerms.Responses;
using ProcHub.Contracts.PaymentTerms.Enums;
using ProcHub.Wpf.Infrastructure.Api;
using ProcHub.Wpf.Infrastructure.Api.Clients;
using ProcHub.Wpf.Infrastructure.Navigation;

namespace ProcHub.Wpf.Features.PaymentTerms.ViewModels;

public sealed partial class PaymentTermsViewModel : PageViewModel
{
    private readonly PaymentTermsApiClient _apiClient;

    public PaymentTermsViewModel(
        PaymentTermsApiClient apiClient)
        : base("Payment terms", "Maintain supplier payment-term master data.")
    {
        _apiClient = apiClient;
    }

    public ObservableCollection<PaymentTermListItemResponse> PaymentTerms
        { get; } = new();

    public IReadOnlyList<PaymentTiming> AvailablePaymentTimings
        { get; } = Enum.GetValues<PaymentTiming>();

    public IReadOnlyList<PaymentDateReference> AvailableDateReferences
        { get; } = Enum.GetValues<PaymentDateReference>();

    [ObservableProperty]
    public partial int? Id { get; private set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial string Description { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial decimal DepositPercentage { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial PaymentTiming? PaymentTiming { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial PaymentDateReference? DueDateReference { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial int BalanceDueDays { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial bool IsCreating { get; private set; }

    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    [NotifyCanExecuteChangedFor(nameof(EditCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    [NotifyPropertyChangedFor(nameof(IsEditorReadOnly))]
    public partial bool IsEditing { get; private set; }
    public bool IsEditorReadOnly => !IsEditing; 


    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    [NotifyCanExecuteChangedFor(nameof(EditCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    public partial PaymentTermResponse? SelectedPaymentTerm { get; set; }

    partial void OnSelectedPaymentTermChanged(
        PaymentTermResponse? value)
    {
        if (value is null)
        {
            return;
        }

        Id = value.Id;
        Description = value.Description;

        DepositPercentage = value
            .DepositPercentage * 100m;

        PaymentTiming = value.PaymentTiming;
        DueDateReference = value.DueDateReference;
        BalanceDueDays = value.BalanceDueDays;

        IsCreating = false;
        IsEditing = false;
    }

    [RelayCommand]
    private void OnNew()
    {
        SelectedPaymentTerm = null;

        Id = null;
        Description = string.Empty;
        DepositPercentage = 0m;
        PaymentTiming = null;
        DueDateReference = null;
        BalanceDueDays = 0;

        IsCreating = true;
        IsEditing = true;

        ClearError();
    }
    private bool FieldsCheck()
    {
        return !string.IsNullOrWhiteSpace(Description)
            && DepositPercentage is >= 0m and <= 100m
            && PaymentTiming is not null
            && DueDateReference is not null
            && BalanceDueDays is >= 0;
    }

    private bool CanEdit()
    {
        return SelectedPaymentTerm is not null
            && !IsEditing;
    }

    [RelayCommand(CanExecute = nameof(CanEdit))]
    private void Edit()
    {
        IsEditing = true;
        ClearError();
    }


    private bool CanSave()
    {
        return IsEditing
            && FieldsCheck()
            && (IsCreating 
                || SelectedPaymentTerm is not null);
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync(
        CancellationToken cancellationToken)
    {
        if (PaymentTiming is null || 
            DueDateReference is null)
        {
            return;
        }

        if (!IsCreating &&
            SelectedPaymentTerm is null)
        {
            return;
        }

        ClearError();

        try
        {
            if (IsCreating)
            {
                await CreateAsync(cancellationToken);
            }
            else
            {
                await UpdateAsync(cancellationToken);
            }

            IsCreating = false;
            IsEditing = false;
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
        CancellationToken cancellationToken)
    {
        var request = new
            CreatePaymentTermRequest(
               Description,
               DepositPercentage / 100m,    
               PaymentTiming!.Value,
               DueDateReference!.Value,
               BalanceDueDays);

        var createdTerm = await _apiClient
            .CreateAsync(
                request,
                cancellationToken);

        var listResponseTransfer = new
            PaymentTermListItemResponse(
                createdTerm.Id,
                createdTerm.Description);

        PaymentTerms.Add(listResponseTransfer);

        SelectedPaymentTerm = createdTerm;
    }

    private async Task UpdateAsync(
        CancellationToken cancellationToken)
    {
        var selectedTerm = SelectedPaymentTerm;

        if (selectedTerm is null ||
            PaymentTiming is null ||
            DueDateReference is null)
        {
            return;
        }
       
        var request = new
            UpdatePaymentTermRequest(
                Description,
                DepositPercentage / 100m,
                PaymentTiming.Value,
                DueDateReference.Value,
                BalanceDueDays);

        var updatedTerm = await _apiClient  
            .UpdateAsync(
                selectedTerm.Id,
                request,
                cancellationToken);

        var updatedListTerm = new
            PaymentTermListItemResponse(
                updatedTerm.Id,
                updatedTerm.Description);

        var existingListTerm = PaymentTerms
            .FirstOrDefault(x => x.Id == updatedTerm.Id);

        if (existingListTerm is not null)
        {
            var index = PaymentTerms.IndexOf(existingListTerm);
            PaymentTerms[index] = updatedListTerm;
        }

        SelectedPaymentTerm = updatedTerm;        
    }

    private bool CanDelete()
    {
        return !IsEditing
            && SelectedPaymentTerm is not null;
    }

    [RelayCommand(CanExecute = nameof(CanDelete))]
    private async Task DeleteAsync(
        CancellationToken cancellationToken)
    {
        var selectedTerm = SelectedPaymentTerm;

        if (selectedTerm is null)
        {
            return;
        }

        var result = MessageBox.Show(
            $"Are you sure you want to delete payment term '{selectedTerm.Description}'?",
            "Delete PaymentTerm warning",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        ClearError();

        try
        {
            await _apiClient
                .DeleteAsync(
                    selectedTerm.Id,
                    cancellationToken);

            var listResponseTransfer = new
                PaymentTermListItemResponse(
                    selectedTerm.Id,
                    selectedTerm.Description);

            PaymentTerms.Remove(listResponseTransfer);

            SelectedPaymentTerm = null;

            Id = null;
            Description = string.Empty;
            DepositPercentage = 0;
            PaymentTiming = null;
            DueDateReference = null;
            BalanceDueDays = 0;
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

    public async Task LoadAllAsync(
        CancellationToken cancellationToken = default)
    {
        ClearError();

        try
        {
            var paymentTerms = await _apiClient
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

    public async Task LoadAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        ClearError();

        try
        {
            var paymentTerm = await _apiClient
                .GetByIdAsync(
                    id,
                    cancellationToken);

            Id = paymentTerm.Id;
            Description = paymentTerm.Description;

            DepositPercentage = paymentTerm
                .DepositPercentage * 100m;

            PaymentTiming = paymentTerm.PaymentTiming;
            DueDateReference = paymentTerm.DueDateReference;
            BalanceDueDays = paymentTerm.BalanceDueDays;
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