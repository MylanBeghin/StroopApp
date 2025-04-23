using StroopApp.ViewModels.Experiment.Experimenter;
using System.Windows.Controls;
using StroopApp.Models;
using LiveChartsCore.SkiaSharpView;

namespace StroopApp.Views.Experiment.Experimenter.Graphs
{
    public partial class GlobalGraphView : UserControl
    {
        public GlobalGraphView(ExperimentGraphViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            var xAxis = new Axis
            {
                MinLimit = -0.5,
                MaxLimit = viewModel.WordCount
            };
            var yAxis = new Axis
            {
                MinLimit = 0,
                MaxLimit = viewModel.MaxReactionTime * 1.1
            };
            GlobalGraph.XAxes = new List<Axis> { xAxis };
            GlobalGraph.YAxes = new List<Axis> { yAxis };
        }
    }
}
