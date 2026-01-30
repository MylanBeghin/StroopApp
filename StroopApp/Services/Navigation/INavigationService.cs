using System.Windows.Controls;

namespace StroopApp.Services.Navigation
{
    /// <summary>
    /// Defines contract for page navigation within the application.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Navigates to a page of the specified type with optional parameter.
        /// </summary>
        void NavigateTo<T>(object? parameter = null) where T : Page;

        /// <summary>
        /// Navigates to a page created by the provided factory.
        /// </summary>
        void NavigateTo(Func<Page> pageFactory);
    }

}
