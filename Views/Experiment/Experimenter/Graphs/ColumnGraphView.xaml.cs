using System.Windows.Controls;
using LiveChartsCore.SkiaSharpView;
using StroopApp.ViewModels.Experiment.Experimenter;

namespace StroopApp.Views.Experiment.Experimenter.Graphs
{
    public partial class ColumnGraphView : UserControl
    {
        public ColumnGraphView(ExperimentGraphViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            var xAxis = new Axis
            {
                MinLimit = -0.5,
                MaxLimit = 10,
                MinStep = 1,
                Labeler = value => ((int)(value+1)).ToString()                
            };
            var yAxis = new Axis
            {
                MinLimit = 0,
                MaxLimit = viewModel.MaxReactionTime * 1.1
            };
            ReactionTimeGraph.XAxes = new List<Axis> { xAxis };
            ReactionTimeGraph.YAxes = new List<Axis> { yAxis };
            viewModel.ReactionPoints.CollectionChanged += (s, e) =>
            {
                int count = viewModel.ReactionPoints.Count;
                const int window = 10;

                if(count > window)
                {
                    xAxis.MinLimit = count - window;
                    xAxis.MaxLimit = count;
                }
            };
        }
    }
}
