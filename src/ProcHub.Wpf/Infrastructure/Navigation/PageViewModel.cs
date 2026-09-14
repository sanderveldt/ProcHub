using ProcHub.Wpf.Infrastructure;

namespace ProcHub.Wpf.Infrastructure.Navigation;

public abstract class PageViewModel : ObservableObject
{
    public string Title { get; }
    public string Description { get; }
    protected PageViewModel(string title, string description)
    {
        Title = title;
        Description = description;
    }
}