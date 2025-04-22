using StroopApp.Models;
using System.Windows.Controls;
using StroopApp.Views.Experiment.Experimenter;
using StroopApp.Services.Navigation;
using StroopApp.ViewModels.Experiment;
using StroopApp.Views.Experiment.Experimenter.Graphs;
using StroopApp.Services.Exportation;

namespace StroopApp.Views
{
    public partial class ExperimentDashBoardPage : Page
    {
        public ExperimentDashBoardPage(ExperimentSettings settings)
        {
            InitializeComponent();
            var ExperimentProfileView = new ExperimentProgressView(settings);
            var GraphsView = new GraphsView(settings);
            DataContext = new ExperimentDashBoardPageViewModel(settings);
            MainGrid.Children.Add(ExperimentProfileView);
            Grid.SetRow(ExperimentProfileView, 1);
            MainGrid.Children.Add(GraphsView);
            Grid.SetRow(GraphsView, 3);
        }
    }
}
