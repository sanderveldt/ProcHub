using System.Windows;
using ProcHub.Wpf.Shell.ViewModels;
using ProcHub.Wpf.Shell.Views;

namespace ProcHub.Wpf.Shell.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}