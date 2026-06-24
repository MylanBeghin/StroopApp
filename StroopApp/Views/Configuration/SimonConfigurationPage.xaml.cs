using StroopApp.Services.Navigation;
using StroopApp.ViewModels.Configuration;
using System.Windows.Controls;

namespace StroopApp.Views.Configuration
{
    /// <summary>
    /// Logique d'interaction pour ConfigurationPage.xaml
    /// </summary>
    public partial class SimonConfigurationPage : Page, INavigationAware
    {
        
        public INavigationService NavigationService
        {
            set => Initialize(value);
        }
        public SimonConfigurationPage()
        {
            InitializeComponent();
        }

        private void Initialize(INavigationService navigationService)
        {
            DataContext = new SimonConfigurationPageViewModel(navigationService);
        }

    }
}
