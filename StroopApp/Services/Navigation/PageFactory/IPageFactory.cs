using System.Windows.Controls;

namespace StroopApp.Services.Navigation.PageFactory
{
    /// <summary>
    /// Factory for creating WPF pages with dependency injection support.
    /// Abstracts page resolution from the DI container.
    /// </summary>
    public interface IPageFactory
    {
        /// <summary>
        /// Creates a page of the specified type, resolving all its dependencies from the DI container.
        /// </summary>
        /// <typeparam name="T">The type of page to create.</typeparam>
        /// <returns>A fully constructed page instance with all dependencies inject
        public T CreatePage<T>() where T : Page;
    }
}
