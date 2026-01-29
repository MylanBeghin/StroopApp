using StroopApp.Models;
using StroopApp.ViewModels.Experiment.Experimenter;
using System.Windows.Controls;

namespace StroopApp.Views.Experiment.Experimenter.Graphs
{
    public partial class GlobalGraphView : UserControl
    {
        public GlobalGraphView(ExperimentSettings settings)
        {
            InitializeComponent();
            DataContext = new GlobalGraphViewModel(settings);
        }
    }
}

