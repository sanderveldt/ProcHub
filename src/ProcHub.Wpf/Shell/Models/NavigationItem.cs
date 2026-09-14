using System.Collections.ObjectModel;
using System.Windows.Input;
using ProcHub.Wpf.Infrastructure;

namespace ProcHub.Wpf.Shell.Models;

public sealed class NavigationItem : ObservableObject
{
    private bool _isExpanded;

    public NavigationItem(
        string title,
        string iconGlyph,
        ICommand? command = null,
        IEnumerable<NavigationItem>? children = null)
    {
        Title = title;
        IconGlyph = iconGlyph;
        Command = command;

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

    public bool IsExpanded
    {
        get => _isExpanded;
        set => SetProperty(ref _isExpanded, value);
    }
}