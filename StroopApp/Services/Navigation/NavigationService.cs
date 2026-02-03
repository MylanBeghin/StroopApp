using System.Windows.Controls;

namespace StroopApp.Services.Navigation
{
    /// <summary>
    /// Service for WPF Frame-based navigation between pages.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private readonly Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
        }
        /// <summary>
        /// Navigates to a page of type T, passing the navigation service and optional parameter to its constructor.
        /// </summary>
        public void NavigateTo<T>(object? parameter = null) where T : Page
        {
            var page = parameter != null
                ? (Page?)Activator.CreateInstance(typeof(T), this, parameter)
                : (Page?)Activator.CreateInstance(typeof(T), this);

            if (page == null)
                throw new InvalidOperationException($"Failed to create instance of {typeof(T).Name}");

            _frame.Navigate(page);
        }
        /// <summary>
        /// Navigates to a page created by the provided factory function.
        /// </summary>
        public void NavigateTo(Func<Page> pageFactory)
        {
            var page = pageFactory();
            _frame.Navigate(page);
        }
    }
}
