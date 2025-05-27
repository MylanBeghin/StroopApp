using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.ViewModels.Experiment;
using StroopApp.Views.Experiment.Experimenter;
using StroopApp.Views.Experiment.Experimenter.Graphs;
using System.Windows.Controls;

namespace StroopApp.Views
{
    public partial class ExperimentDashBoardPage : Page
    {
        public ExperimentDashBoardPage(ExperimentSettings settings, INavigationService experimenterNavigationService, IWindowManager windowManager)
        {
            InitializeComponent();
            var ExperimentProfileView = new ExperimentProgressView(settings);
            var GraphsView = new GraphsView(settings);
            DataContext = new ExperimentDashBoardPageViewModel(settings, experimenterNavigationService, windowManager);
            MainGrid.Children.Add(ExperimentProfileView);
            Grid.SetRow(ExperimentProfileView, 1);
            MainGrid.Children.Add(GraphsView);
            Grid.SetRow(GraphsView, 3);
        }
    }
}
