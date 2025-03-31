using System.Windows;
using StroopApp.ViewModels;
using StroopApp.Services.Navigation;
using StroopApp.Models;
namespace StroopApp.Views
{
    public partial class ExperimentWindow : Window
    {
        public INavigationService NavigationService { get; private set; }
        public ExperimentWindow()
        {
            InitializeComponent();
            var Settings = new ExperimentSettings();
            NavigationService = new NavigationService(MainFrame);
            var configPage = new ConfigurationPage(NavigationService);
            NavigationService.NavigateTo(() => configPage);
            var experimentPage = new ExperimentDashBoardPage(NavigationService, Settings);
            DataContext = new ExperimentWindowViewModel();
        }

    }
}
