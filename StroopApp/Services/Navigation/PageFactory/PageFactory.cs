using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace StroopApp.Services.Navigation
{
    /// <summary>
    /// Resolves WPF pages from the DI container, ensuring all constructor dependencies are injected.
    /// </summary>
    public class PageFactory : IPageFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new PageFactory with the application's service provider.
        /// </summary>
        /// <param name="serviceProvider">The DI service provider used to resolve page instances.</param>
        public PageFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        }

        /// <inheritdoc/>
        public T CreatePage<T>() where T : Page
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
