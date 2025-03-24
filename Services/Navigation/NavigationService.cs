using StroopApp.Services.Navigation;
using System;
using System.Windows.Controls;

public class NavigationService : INavigationService
{
    private readonly Frame _frame;

    public NavigationService(Frame frame)
    {
        _frame = frame;
    }

    public void NavigateTo<T>(object parameter = null) where T : Page
    {
        // Crée une instance de la page.
        // Si la page a besoin d'un paramètre dans son constructeur, elle doit l'accepter.
        var page = parameter != null
            ? (Page)Activator.CreateInstance(typeof(T), parameter)
            : (Page)Activator.CreateInstance(typeof(T));
        _frame.Navigate(page);
    }
}
