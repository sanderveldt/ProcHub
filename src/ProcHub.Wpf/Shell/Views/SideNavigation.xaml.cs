using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using ProcHub.Wpf.Shell.Models;
using ProcHub.Wpf.Shell.ViewModels;

namespace ProcHub.Wpf.Shell.Views;

public partial class SideNavigation : UserControl
{
    public SideNavigation()
    {
        InitializeComponent();
    }

    private void NavigationTree_PreviewMouseLeftButtonUp(
        object sender,
        MouseButtonEventArgs e)
    {
        if (FindAncestor<ToggleButton>(e.OriginalSource as DependencyObject) is not null)
        {
            return;
        }

        var itemContainer = 
            FindAncestor<TreeViewItem>(e.OriginalSource as DependencyObject);
        
        if (itemContainer?.DataContext is not NavigationItem item)
        {
            return;
        }

        if (item.Children.Count > 0)
        {
            item.IsExpanded = !item.IsExpanded;
        }
        else if (item.Command?.CanExecute(null) == true)
        {
            item.Command.Execute(null);
        }

        e.Handled = true;
    }

    private void CompactItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button { DataContext: NavigationItem item })
        {
            return;
        }

        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.OpenCompactItem(item);
        } 
    }

    private static T? FindAncestor<T>(DependencyObject? current)
        where T : DependencyObject
    {
        while (current is not null)
        {
            if (current is T result)
            {
                return result;
            }

            current = VisualTreeHelper.GetParent(current);
        }

        return null;
    }
}