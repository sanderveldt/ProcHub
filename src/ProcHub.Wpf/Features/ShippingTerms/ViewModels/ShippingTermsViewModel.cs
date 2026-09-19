using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcHub.Contracts.ShippingTerms.Requests;
using ProcHub.Contracts.ShippingTerms.Responses;
using ProcHub.Wpf.Infrastructure.Api;
using ProcHub.Wpf.Infrastructure.Api.Clients;
using ProcHub.Wpf.Infrastructure.Navigation;


namespace ProcHub.Wpf.Features.ShippingTerms.ViewModels;

public sealed partial class ShippingTermsViewModel : PageViewModel
    
{
    private readonly ShippingTermsApiClient _apiClient;
    
    public ShippingTermsViewModel(
        ShippingTermsApiClient apiClient)
        : base(
            "Shipping Terms",
            "Maintain shipping terms.")
    {
        _apiClient = apiClient;
    }

    public ObservableCollection<ShippingTermResponse> ShippingTerms 
        { get; } = new();

    [ObservableProperty]
    public partial int? Id { get; private set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial string Description { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial bool IsCreating { get; private set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    public partial ShippingTermResponse? SelectedShippingTerm { get; set; }

    partial void OnSelectedShippingTermChanged(
        ShippingTermResponse? value)
    {
        if (value is null)
        {
            return;
        }

        Id = value.Id;
        Name = value.Name;
        Description = value.Description;
        IsCreating = false;
    }

    public async Task LoadAllAsync(
        CancellationToken cancellationToken = default)
    {
        var shippingTerms = await _apiClient
            .GetAllAsync(cancellationToken);
        
        ShippingTerms.Clear();

        foreach (var shippingTerm in shippingTerms)
        {
            ShippingTerms.Add(shippingTerm);
        }
    }

    [RelayCommand]
    private void OnNew()
    {
        SelectedShippingTerm = null;

        Id = null;
        Name = string.Empty;
        Description = string.Empty;

        IsCreating = true;
    }

    private bool CanSave()
    {
        return IsCreating &&
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(Description);
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync(
        CancellationToken cancellationToken)
    {
        if (!IsCreating)
        {
            return;
        }

        var request = new
            CreateShippingTermRequest(
                Name,
                Description);

        var created = await _apiClient
            .CreateAsync(
                request,
                cancellationToken);

        ShippingTerms.Add(created);

        Id = created.Id;
        Name = created.Name;
        Description = created.Description;

        IsCreating = false;
    }

    private bool CanDelete()
    {
        return SelectedShippingTerm 
            is not null;
    }

    [RelayCommand(CanExecute = nameof(CanDelete))]
    private async Task DeleteAsync(
        CancellationToken cancellationToken)
    {
        var selectedTerm = SelectedShippingTerm;

        if (selectedTerm is null)
        {
            return;
        }

        var result = MessageBox.Show(
            $"Are you sure you want to delete shipping term '{selectedTerm.Name}'?",
            "Delete:",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        await _apiClient.DeleteAsync(
            selectedTerm.Id,
            cancellationToken);

        ShippingTerms.Remove(selectedTerm);

        SelectedShippingTerm = null;

        Id = null;
        Name = string.Empty;
        Description = string.Empty;
    }

    public async Task LoadAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var shippingTerm = await _apiClient
            .GetbyIdAsync(
                id,
                cancellationToken);
        
        Id = shippingTerm.Id;
        Name = shippingTerm.Name;
        Description = shippingTerm.Description;
    }
}