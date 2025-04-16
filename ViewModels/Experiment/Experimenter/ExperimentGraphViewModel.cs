using System.Collections.ObjectModel;
using LiveChartsCore;
using StroopApp.Models;

namespace StroopApp.ViewModels.Experiment.Experimenter
{
    public class ExperimentGraphViewModel
    {

        public ObservableCollection<ISeries> ColumnSerie { get; set; }
        public ObservableCollection<ISeries> GlobalSerie { get; set; }
        public ObservableCollection<ReactionTimePoint> ReactionPoints { get; set; }
        public int WordCount { get; set; }
        public int MaxReactionTime { get; set; }
        public ExperimentGraphViewModel(ExperimentSettings settings)
        {
            ReactionPoints = settings.ExperimentContext.ReactionPoints;
            ColumnSerie = settings.ExperimentContext.ColumnSerie;
            GlobalSerie = settings.ExperimentContext.GlobalSerie;
            WordCount = settings.CurrentProfile.WordCount;
            MaxReactionTime = settings.CurrentProfile.MaxReactionTime;
        }
    }
}
