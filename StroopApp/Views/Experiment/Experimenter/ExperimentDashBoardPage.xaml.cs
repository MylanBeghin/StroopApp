using StroopApp.Models;
using StroopApp.Services.Language;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.ViewModels.Experiment.Experimenter;
using StroopApp.Views.Experiment.Experimenter;
using StroopApp.Views.Experiment.Experimenter.Graphs;
using System.Windows.Controls;

namespace StroopApp.Views
{
    public partial class ExperimentDashBoardPage : Page
    {
        public ExperimentDashBoardPage(ExperimentSettings settings, INavigationService experimenterNavigationService, IWindowManager windowManager, ILanguageService languageService)
        {
            InitializeComponent();
            var experimentProfileView = new ExperimentProgressView(settings);
            var graphsView = new GraphsView(settings);
            DataContext = new ExperimentDashBoardPageViewModel(settings, experimenterNavigationService, windowManager, languageService);
            MainGrid.Children.Add(experimentProfileView);
            Grid.SetRow(experimentProfileView, 1);
            MainGrid.Children.Add(graphsView);
            Grid.SetRow(graphsView, 3);
        }
    }
}
