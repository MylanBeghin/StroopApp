using StroopApp.Models;
using StroopApp.ViewModels.Experiment.Experimenter;
using StroopApp.ViewModels.State;
using System.Windows.Controls;

namespace StroopApp.Views.Experiment.Experimenter.Graphs
{
    public partial class GlobalGraphView : UserControl
    {
        public GlobalGraphView(ExperimentSettingsViewModel settings)
        {
            InitializeComponent();
            DataContext = new GlobalGraphViewModel(settings);
        }
    }
}

