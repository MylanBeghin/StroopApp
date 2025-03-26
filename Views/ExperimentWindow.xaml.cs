using System.Windows;
using StroopApp.ViewModels;
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
            var experimentPage = new ExperimentDashBoardPage(NavigationService);
            NavigationService.NavigateTo(() => configPage);
            DataContext = new ExperimentWindowViewModel();
        }

    }
}
