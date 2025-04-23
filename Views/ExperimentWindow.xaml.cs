using System.Windows;
using StroopApp.Models;
using StroopApp.Services.Exportation;
using StroopApp.Services.Navigation;
using StroopApp.ViewModels;
using StroopApp.Views.Experiment.Experimenter;

namespace StroopApp.Views
{
    public partial class ExperimentWindow : Window
    {
        public ExperimentSettings Settings { get; }

        public ExperimentWindow(ExperimentSettings settings)
        {
            InitializeComponent();
            Settings = settings;
            App.ExperimentWindowNavigationService = new NavigationService(MainFrame);

            var configPage = new ConfigurationPage();
            App.ExperimentWindowNavigationService.NavigateTo(() => configPage);

            var experimentPage = new ExperimentDashBoardPage(Settings);

            DataContext = new ExperimentWindowViewModel(
                configPage,
                experimentPage);
        }
    }
}
