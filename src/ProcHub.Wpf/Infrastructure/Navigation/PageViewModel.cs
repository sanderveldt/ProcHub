using ProcHub.Wpf.Infrastructure;

namespace ProcHub.Wpf.Infrastructure.Navigation;

public abstract class PageViewModel : ObservableObject
{
    public string Title { get; }
    public string Description { get; }
    protected PageViewModel(
        string title,
        string description,
        params string[] allowedRoles)
    {
        Title = title;
        Description = description;
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