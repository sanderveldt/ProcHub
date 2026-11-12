using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcHub.Contracts.Users.Requests;
using ProcHub.Contracts.Users.Responses;
using ProcHub.Wpf.Infrastructure.Api;
using ProcHub.Wpf.Infrastructure.Api.Clients;
using ProcHub.Wpf.Infrastructure.Authentication;
using ProcHub.Wpf.Infrastructure.Navigation;
using System.Collections.ObjectModel;
using System.Windows;

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
    public partial int? Id { get; private set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial string UserName { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial string? DisplayName { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial string? Role { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    [NotifyPropertyChangedFor(nameof(CanEditFields))]
    [NotifyPropertyChangedFor(nameof(IsEditorReadOnly))]
    public partial bool IsCreating { get; private set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    [NotifyCanExecuteChangedFor(nameof(EditCommand))]
    [NotifyPropertyChangedFor(nameof(CanEditFields))]
    [NotifyPropertyChangedFor(nameof(IsEditorReadOnly))]
    public partial bool IsEditing { get; private set; }

    public bool IsEditorReadOnly => !CanEditFields;
    public bool CanEditFields => IsCreating || IsEditing;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    [NotifyCanExecuteChangedFor(nameof(EditCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    public partial UserResponse? SelectedUser { get; set; }

    async partial void OnSelectedUserChanged(
        UserResponse? value)
    {
        if (value is null)
        {
            return;
        }

        await LoadUserAsync(value.Id);

        IsCreating = false;
        IsEditing = false;
    }

    private bool FieldsCheck()
    {
        return !string.IsNullOrWhiteSpace(Email)
            && !string.IsNullOrWhiteSpace(Role);
    }

    [RelayCommand]
    private void OnNew()
    {
        SelectedUser = null;

        Id = null;
        UserName = string.Empty;
        Email = string.Empty;
        DisplayName = string.Empty;
        Role = string.Empty;

        IsCreating = true;
        IsEditing = true;

        ClearError();
    }

    private bool CanEdit()
    {
        return SelectedUser is not null
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
                || SelectedUser is not null);
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    public async Task SaveAsync(
        string? password,
        CancellationToken cancellationToken)
    {
        if (!IsCreating && !IsEditing)
        {
            return;
        }

        ClearError();

        try
        {
            if (IsCreating)
            {
                await CreateAsync(
                    password ?? string.Empty,
                    cancellationToken);
            }
            else
            {
                await UpdateAsync(
                    password,
                    cancellationToken);
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
        string? newPassword,
        CancellationToken cancellationToken = default)
    {
        var selectedUser = SelectedUser;

        if (selectedUser is null)
        {
            return;
        }

        var request = new UpdateUserRequest(
            UserName,
            Role!)
        {
            DisplayName = DisplayName,
            NewPassword = string.IsNullOrEmpty(newPassword)
                ? null
                : newPassword
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

    private bool CanDelete()
    {
        return !IsCreating
            && !IsEditing
            && SelectedUser is not null;
    }

    [RelayCommand(CanExecute = nameof(CanDelete))]
    public async Task DeleteAsync(
        CancellationToken cancellationToken = default)
    {
        var selectedUser = SelectedUser;

        if (selectedUser is null)
        {
            return;
        }

        var result = MessageBox.Show(
            $"Are you sure you want to delete User '{selectedUser.UserName}'?",
            "Delete User",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
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