using System.Windows;
using System.Windows.Controls;
using StroopApp.Services;
using StroopApp.Services.Navigation;

namespace StroopApp.Views
{
    public partial class ExperimentWindow : Window
    {
        public INavigationService NavigationService { get; private set; }
        public ExperimentWindow()
        {
            InitializeComponent();
            NavigationService = new NavigationService(MainFrame);
            var configPage = new ConfigurationPage(NavigationService);
            NavigationService.NavigateTo(() => configPage);
        }

    }
}
