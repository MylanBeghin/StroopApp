using StroopApp.Models;
using System.Windows.Controls;
using StroopApp.Views.Experiment.Experimenter;
using StroopApp.Services.Navigation;
using StroopApp.ViewModels.Experiment;
namespace StroopApp.Views
{
    public partial class ExperimentDashBoardPage : Page
    {
        public ExperimentDashBoardPage(INavigationService navigationService, ExperimentSettings settings)
        {
            InitializeComponent();
            
            var ExperimentProfileView = new ExperimentProgressView(settings);
            var ExperimentGraphView = new ExperimentGraphView(settings);
            DataContext = new ExperimentDashBoardPageViewModel(navigationService, settings);
            MainGrid.Children.Add(ExperimentProfileView);
            Grid.SetRow(ExperimentProfileView, 1);
            MainGrid.Children.Add(ExperimentGraphView);
            Grid.SetRow(ExperimentGraphView, 3);
        }
    }
}
