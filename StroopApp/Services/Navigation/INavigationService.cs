using StroopApp.Services.Navigation;
using System;
using System.Windows.Controls;

namespace StroopApp.Services.Navigation
{
    public interface INavigationService
    {
        void NavigateTo<T>(object? parameter = null) where T : System.Windows.Controls.Page;
        void NavigateTo(Func<Page> pageFactory);
    }

}
