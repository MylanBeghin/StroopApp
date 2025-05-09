using StroopApp.Models;
using StroopApp.ViewModels.Experiment.Experimenter;
using System.Windows.Controls;

namespace StroopApp.Views.Experiment.Experimenter.Graphs
{
    public partial class ColumnGraphView : UserControl
    {
        public ColumnGraphView(ExperimentSettings settings)
        {
            InitializeComponent();
            DataContext = new ColumnGraphViewModel(settings);
        }
    }
}
