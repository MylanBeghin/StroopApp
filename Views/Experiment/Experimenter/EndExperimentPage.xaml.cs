using StroopApp.Models;
using StroopApp.Services.Exportation;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.ViewModels.Experiment.Experimenter;
using StroopApp.Views.Experiment.Experimenter.Graphs;
using System.Windows.Controls;

namespace StroopApp.Views.Experiment.Experimenter
{
    public partial class EndExperimentPage : Page
    {

        public EndExperimentPage(ExperimentSettings settings, INavigationService experimenterNavigationService, IWindowManager windowManager)
        {
            InitializeComponent();
            var ExportationService = new ExportationService(settings);
            DataContext = new EndExperimentViewModel(settings, ExportationService, experimenterNavigationService, windowManager);
            var GlobalGraph = new GlobalGraphView(settings);
            MainGrid.Children.Add(GlobalGraph);
            Grid.SetRow(GlobalGraph, 4);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
