using ProcHub.Contracts.ShippingTerms.Requests;
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
        set => SetProperty(ref _description, value);
    }

    public bool IsCreating
    {
        get => _isCreating;
        private set =>
            SetProperty(ref _isCreating, value);

    }

    public RelayCommand NewCommand { get; }
    public AsyncRelayCommand SaveCommand { get; }

    

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

        Id = created.Id;
        Name = created.Name;
        Description_ = created.Description;

        IsCreating = false;

        SaveCommand.RaiseCanExecuteChanged();
    }

    private async Task LoadAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var shippingTerm = await _apiClient
            .GetbyIdASync(
                id, cancellationToken);

        Id = shippingTerm.Id;
        Name = shippingTerm.Name;
        Description_ = shippingTerm.Description;
    }

    private async Task CreateAsync(
        CancellationToken cancellationToken = default)
    {
        var request =
            new CreateShippingTermRequest(
                Name,
                Description);

        var created = await _apiClient
            .CreateAsync(
                request,
                cancellationToken);

        Id = created.Id;
        Name = created.Name;
        Description_ = created.Description;
    }
}