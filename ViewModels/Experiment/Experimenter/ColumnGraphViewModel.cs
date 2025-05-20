using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using StroopApp.Models;
using System.Collections.ObjectModel;

namespace StroopApp.ViewModels.Experiment.Experimenter
{

    internal class ColumnGraphViewModel
    {
        public ObservableCollection<ISeries> ColumnSerie
        {
            get;
        }
        public Axis[] XAxes
        {
            get;
        }
        public Axis[] YAxes
        {
            get;
        }
        public ColumnGraphViewModel(ExperimentSettings settings)
        {
            ColumnSerie = settings.ExperimentContext.ColumnSerie;
            XAxes = new[]
            {
                new Axis
                {
                    MinLimit = -0.5,
                    MaxLimit = 10,
                    MinStep = 1,
                    Labeler = value => ((int)(value + 1)).ToString()
                }
            };
            YAxes = new[]
            {
                new Axis
                {
                    MinLimit = 0,
                    MaxLimit = settings.CurrentProfile.MaxReactionTime * 1.1
                }
            };
            settings.ExperimentContext.ReactionPoints.CollectionChanged += (s, e) =>
            {
                int count = settings.ExperimentContext.ReactionPoints.Count;
                const int window = 10;

                if (count > window)
                {
                    XAxes.First().MinLimit = count - window;
                    XAxes.First().MaxLimit = count;
                }
                ;
            };

        }
    }
}