using CommunityToolkit.Mvvm.ComponentModel;
using ProcHub.Wpf.Infrastructure;

namespace ProcHub.Wpf.Infrastructure.Navigation;

public abstract class PageViewModel : ObservableObject
{
    public string Title { get; }
    public string PageDescription { get; }
    protected PageViewModel(
        string title,
        string pageDescription,
        params string[] allowedRoles)
    {
        Title = title;
        PageDescription = pageDescription;
        AllowedRoles = allowedRoles;
    }

    public IReadOnlyCollection<string> AllowedRoles { get; }

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
}