using CommunityToolkit.Mvvm.ComponentModel;
using ProcHub.Contracts.Users.Requests;
using ProcHub.Contracts.Users.Responses;
using ProcHub.Wpf.Infrastructure.Api;
using ProcHub.Wpf.Infrastructure.Api.Clients;
using ProcHub.Wpf.Infrastructure.Authentication;
using ProcHub.Wpf.Infrastructure.Navigation;
using System.Collections.ObjectModel;

namespace ProcHub.Wpf.Features.Users.ViewModels;

public sealed partial class UsersViewModel : PageViewModel
{
    private readonly UsersApiClient _usersApiClient;

    public UsersViewModel(
        UsersApiClient usersApiClient)
        : base(
            "Users",
            "Maintain users and user settings.",
            AppRoles.SuperUser,
            AppRoles.Admin)
    {
        _usersApiClient = usersApiClient;
    }

    public ObservableCollection<UserResponse> Users
    { get; } = new();

    public IReadOnlyList<string> Roles => AppRoles.All;

    [ObservableProperty]
    public partial int Id { get; set; }

    [ObservableProperty]
    public partial string UserName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string? DisplayName { get; set; }

    [ObservableProperty]
    public partial string? Role { get; set; }

    [ObservableProperty]
    public partial bool IsCreating { get; private set; }

    [ObservableProperty]
    public partial bool IsEditing { get; private set; }

    public bool IsEditorReadOnly => !IsCreating || !IsEditing;

    [ObservableProperty]
    public partial UserResponse? SelectedUser { get; set; }

    async partial void OnSelectedUserChanged(
        UserResponse? value)
    {
        if (value is null)
        {
            return;
        }
    }



    public async Task LoadAllUsersAsync(
        CancellationToken cancellationToken = default)
    {
        ClearError();

        try 
        {
            var users = await _usersApiClient
                .GetAllAsync(cancellationToken);

            Users.Clear();

            foreach (var user in users)
            {
                Users.Add(user);
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

    public async Task LoadUserAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        ClearError();

        try
        {
            var user = await _usersApiClient
                .GetByIdAsync(
                    id,
                    cancellationToken);

            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            DisplayName = user.DisplayName;
            Role = user.Role;
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
        string password,
        CancellationToken cancellationToken = default)
    {
        var request = new CreateUserRequest(
            Email,
            password,
            Role!)
        {
            DisplayName = DisplayName

        };

        var newUser = await _usersApiClient
            .CreateAsync(
                request,
                cancellationToken);

        Users.Add(newUser);

        SelectedUser = newUser;
    }

    public async Task UpdateAsync(
        CancellationToken cancellationToken = default)
    {
        var selectedUser = SelectedUser;

        if (selectedUser is null)
        {
            return;
        }

        var request = new UpdateUserRequest(
            UserName,
            // add password to api endpoint for reset
            Role!)
        {
            DisplayName = DisplayName
        };

        var updatedUser = await _usersApiClient
            .UpdateAsync(
                selectedUser.Id,
                request,
                cancellationToken);

        Users.Remove(selectedUser);
        Users.Add(updatedUser);

        SelectedUser = updatedUser;
    }

    public async Task DeleteAsync(
        CancellationToken cancellationToken = default)
    {
        var selectedUser = SelectedUser;

        if (selectedUser is null)
        {
            return;
        }

        try
        {
            await _usersApiClient
                .DeleteAsync(
                    selectedUser.Id,
                    cancellationToken);

            Users.Remove(selectedUser);

            SelectedUser = null;
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