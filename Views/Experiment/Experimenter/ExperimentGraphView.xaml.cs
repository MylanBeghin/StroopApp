using StroopApp.ViewModels.Experiment.Experimenter;
using System.Windows.Controls;
using StroopApp.Models;
using LiveChartsCore.SkiaSharpView;

namespace StroopApp.Views.Experiment.Experimenter
{
    public partial class ExperimentGraphView : UserControl
    {
        public ExperimentGraphView(ExperimentSettings settings)
        {
            InitializeComponent();
            var viewModel = new ExperimentGraphViewModel(settings);
            DataContext = viewModel;
            var xAxis = new Axis
            {
                MinLimit = 0,
                MaxLimit = 10,
                MinStep = 1,
                Labeler = value => ((int)(value+1)).ToString()                
            };
            var yAxis = new Axis
            {
                MinLimit = 0,
                MaxLimit = settings.CurrentProfile.MaxReactionTime*1.1            };
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
