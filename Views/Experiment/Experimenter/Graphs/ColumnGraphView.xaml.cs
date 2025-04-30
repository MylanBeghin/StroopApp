using LiveChartsCore.SkiaSharpView;
using StroopApp.Models;
using System.Windows.Controls;

namespace StroopApp.Views.Experiment.Experimenter.Graphs
{
    public partial class ColumnGraphView : UserControl
    {
        private readonly ExperimentSettings _settings;
        public ColumnGraphView(ExperimentSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            DataContext = _settings.ExperimentContext;
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
                MaxLimit = _settings.CurrentProfile.MaxReactionTime * 1.1
            };
            ReactionTimeGraph.XAxes = new List<Axis> { xAxis };
            ReactionTimeGraph.YAxes = new List<Axis> { yAxis };
            _settings.ExperimentContext.ReactionPoints.CollectionChanged += (s, e) =>
            {
                int count = _settings.ExperimentContext.ReactionPoints.Count;
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
