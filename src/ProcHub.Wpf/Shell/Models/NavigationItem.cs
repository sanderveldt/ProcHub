using System.Collections.ObjectModel;
using System.Windows.Input;
using ProcHub.Wpf.Infrastructure;
using ProcHub.Wpf.Infrastructure.Authentication;

namespace ProcHub.Wpf.Shell.Models;

public sealed class NavigationItem : ObservableObject
{
    private bool _isExpanded;

    public NavigationItem(
        string title,
        string iconGlyph,
        ICommand? command = null,
        IEnumerable<NavigationItem>? children = null,
        IEnumerable<string>? allowedRoles = null)
    {
        Title = title;
        IconGlyph = iconGlyph;
        Command = command;

        AllowedRoles = 
            allowedRoles?.ToArray()
            ?? Array.Empty<string>();

        if (children is null)
        {
            return;
        }

        foreach (var child in children)
        {
            Children.Add(child);
        }
    }

    public string Title { get; }
    public string IconGlyph { get; }
    public ICommand? Command { get; }

    public ObservableCollection<NavigationItem> Children { get; } = [];

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

    public bool IsExpanded
    {
        get => _isExpanded;
        set => SetProperty(ref _isExpanded, value);
    }
}