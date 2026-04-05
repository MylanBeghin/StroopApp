using StroopApp.Models;
using StroopApp.ViewModels.Experiment.Experimenter;
using StroopApp.ViewModels.State;
using System.Windows.Controls;

namespace StroopApp.Views.Experiment.Experimenter.Graphs
{
    public partial class ColumnGraphView : UserControl
    {
        public ColumnGraphView(ExperimentSettingsViewModel settings)
        {
            InitializeComponent();
            DataContext = new ColumnGraphViewModel(settings);
        }
    }
}
