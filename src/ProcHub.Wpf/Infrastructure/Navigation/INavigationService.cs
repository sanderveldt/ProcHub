using System.ComponentModel;
using System.Windows;

namespace ProcHub.Wpf.Infrastructure.Navigation;

public interface INavigationService : INotifyPropertyChanged
{
    PageViewModel? CurrentViewModel { get; }

    void NavigateTo<TViewModel>()
        where TViewModel : PageViewModel;
}