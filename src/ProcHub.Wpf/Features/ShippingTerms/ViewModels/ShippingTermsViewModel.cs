using System.Collections.ObjectModel;
using ProcHub.Contracts.ShippingTerms.Requests;
using ProcHub.Contracts.ShippingTerms.Responses;
using ProcHub.Wpf.Infrastructure;
using ProcHub.Wpf.Infrastructure.Api.Clients;
using ProcHub.Wpf.Infrastructure.Navigation;


namespace ProcHub.Wpf.Features.ShippingTerms.ViewModels;

public sealed class ShippingTermsViewModel : PageViewModel
    
{
    private readonly ShippingTermsApiClient _apiClient;
    private int? _id;
    private string _name = string.Empty;
    private string _description = string.Empty;
    private bool _isCreating;

    public ShippingTermsViewModel(
        ShippingTermsApiClient ApiClient)
        : base(
            "Shipping Terms",
            "Maintain shipping terms.")
    {
        _apiClient = ApiClient;

        NewCommand = new RelayCommand(StartNew);

        SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
    }

    public ObservableCollection<ShippingTermResponse> ShippingTerms { get; } = new();

    public int? Id
    {
        get => _id;
        private set => SetProperty(ref _id, value);
    }

    public string Name
    {
        get => _name;
        set
        {
            if (SetProperty(ref _name, value))
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
    }


    // Will remove _ once I remove description from PageViewModel
    public string Description_
    {
        get => _description;
        set
        { 
            if (SetProperty(ref _description, value))
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
        
        }
    }

    public bool IsCreating
    {
        get => _isCreating;
        private set
        {
            if (SetProperty(ref _isCreating, value))
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public RelayCommand NewCommand { get; }
    public AsyncRelayCommand SaveCommand { get; }

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

    

    private void StartNew()
    {
        Id = null;
        Name = string.Empty;
        Description_ = string.Empty;

        IsCreating = true;

        SaveCommand.RaiseCanExecuteChanged();
    }

    private bool CanSave()
    {
        return IsCreating &&
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(Description_);
    }

    private async Task SaveAsync()
    {
        if (!IsCreating)
        {
            return;
        }

        var request =
            new CreateShippingTermRequest(
                Name,
                Description_);

        var created = await _apiClient.CreateAsync(request);

        ShippingTerms.Add(created);

        Id = created.Id;
        Name = created.Name;
        Description_ = created.Description;

        IsCreating = false;

        SaveCommand.RaiseCanExecuteChanged();
    }



    public async Task LoadAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var shippingTerm = await _apiClient
            .GetbyIdAsync(
                id, cancellationToken);

        Id = shippingTerm.Id;
        Name = shippingTerm.Name;
        Description_ = shippingTerm.Description;
    }

    public async Task CreateAsync(
        CancellationToken cancellationToken = default)
    {
        var request =
            new CreateShippingTermRequest(
                Name,
                Description_);

        var created = await _apiClient
            .CreateAsync(
                request,
                cancellationToken);

        Id = created.Id;
        Name = created.Name;
        Description_ = created.Description;
    }
}