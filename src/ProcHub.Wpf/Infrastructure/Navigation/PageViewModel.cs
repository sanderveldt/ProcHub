using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq.Expressions;

namespace ProcHub.Wpf.Infrastructure.Navigation;

public abstract partial class PageViewModel : ObservableObject
{
    public string Title { get; }
    public string PageDescription { get; }

    public IReadOnlyCollection<string> AllowedRoles { get; }

    private CancellationTokenSource? _errorMessageCTSource;

    protected PageViewModel(
        string title,
        string pageDescription,
        params string[] allowedRoles)
    {
        Title = title;
        PageDescription = pageDescription;
        AllowedRoles = allowedRoles;
    }

    [ObservableProperty]
    public partial string? ErrorMessage { get; private set; }

    public bool IsAllowedFor(string? role)
    {
        if (AllowedRoles.Count == 0)
        {
            return true;
        }

        if (string.IsNullOrWhiteSpace(role))
        {
            return false;
        }

        return AllowedRoles.Contains(
            role,
            StringComparer.OrdinalIgnoreCase);
    }

    protected void ShowError(
        string message,
        TimeSpan? duration = null)
    {
        ClearError();

        ErrorMessage = message;

        var cancellationTokenSource = new
            CancellationTokenSource();

        _errorMessageCTSource = cancellationTokenSource;

        _ = ClearErrorAfterDelayAsync(
                cancellationTokenSource,
                // optional duration input:
                duration
                // default delay:
                ?? TimeSpan.FromSeconds(5));
    }

    protected void ClearError()
    {
        _errorMessageCTSource?.Cancel();
        _errorMessageCTSource?.Dispose();
        _errorMessageCTSource = null;

        ErrorMessage = null;
    }

    private async Task ClearErrorAfterDelayAsync(
        CancellationTokenSource cancellationTokenSource,
        TimeSpan delay)
    {
        try
        {
            // delay unless cancelled
            await Task.Delay(
                delay,
                cancellationTokenSource.Token);

            // concurrency safety check
            if (ReferenceEquals(
                    _errorMessageCTSource,
                    cancellationTokenSource))
            {
                ErrorMessage = null;

                _errorMessageCTSource.Dispose();
                _errorMessageCTSource = null;
            }
        }
        catch (OperationCanceledException)
        {
            // expected, continue
        }
    }
}