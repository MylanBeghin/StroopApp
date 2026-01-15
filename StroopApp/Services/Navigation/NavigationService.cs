using StroopApp.Services.Navigation;
using System;
using System.Windows.Controls;

public class NavigationService : INavigationService
{
    private readonly Frame _frame;

    public NavigationService(Frame frame)
    {
        _frame = frame ?? throw new ArgumentNullException(nameof(frame));
    }

    public void NavigateTo<T>(object? parameter = null) where T : Page
{
    // Si le paramètre est fourni, on injecte également le service de navigation (this) en premier argument.
    var page = parameter != null
        ? (Page?)Activator.CreateInstance(typeof(T), this, parameter)
        : (Page?)Activator.CreateInstance(typeof(T), this);
    
    if (page == null)
        throw new InvalidOperationException($"Failed to create instance of {typeof(T).Name}");
    
    _frame.Navigate(page);
}

    public void NavigateTo(Func<Page> pageFactory)
    {
        var page = pageFactory();
        _frame.Navigate(page);
    }
}
