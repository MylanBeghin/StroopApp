using System.Windows;
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
            NavigationService.NavigateTo<ConfigurationPage>();
        }
    }
}
