using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using StroopApp.Models;

namespace StroopApp.ViewModels.Experiment.Experimenter
{
    public class ExperimentGraphViewModel
    {

        public ObservableCollection<ISeries> Series { get; set; }
        public ObservableCollection<ReactionTimePoint> ReactionPoints { get; set; }

        public ExperimentGraphViewModel(ExperimentSettings settings)
        {
            ReactionPoints = settings.ExperimentContext.ReactionPoints;
            Series = settings.ExperimentContext.Series;
        }
    }
}
