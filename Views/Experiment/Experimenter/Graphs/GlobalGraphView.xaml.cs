using StroopApp.ViewModels.Experiment.Experimenter;
using System.Windows.Controls;
using LiveChartsCore.SkiaSharpView;
using StroopApp.Models;

namespace StroopApp.Views.Experiment.Experimenter.Graphs
{
    public partial class GlobalGraphView : UserControl
    {
        private readonly ExperimentSettings _settings;

        public GlobalGraphView(ExperimentSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            DataContext = _settings.ExperimentContext;
            var xAxis = new Axis
            {
                MinLimit = -0.5,
                MaxLimit = _settings.ExperimentContext.TrialRecords.Count() + _settings.CurrentProfile.WordCount,
                MinStep = 1
            };
            var maxReactionTime = _settings.ExperimentContext.TrialRecords
            .Where(p => p.ReactionTime.HasValue)
            .Select(p => p.ReactionTime.Value)
            .DefaultIfEmpty(0)
            .Max();

            var yAxis = new Axis
            {
                MinLimit = 0,
                MaxLimit = Math.Max(maxReactionTime, _settings.CurrentProfile.MaxReactionTime)
            };
            GlobalGraph.XAxes = new List<Axis> { xAxis };
            GlobalGraph.YAxes = new List<Axis> { yAxis };
            ;
        }
    }
}
