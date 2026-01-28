using StroopApp.Core;
using StroopApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StroopApp.ViewModels.Experiment.Experimenter
{
    public class ExperimentProgressViewModel : ViewModelBase
    {
        public ExperimentSettings settings
        {
            get;
        }
        public int Progress => (settings.CurrentProfile.WordCount > 0) && (settings.ExperimentContext != null)
    ? (int)(((double)settings.ExperimentContext.ReactionPoints.Count / settings.CurrentProfile.WordCount) * 100)
        : 0;

        public ObservableCollection<StroopTrial> TrialRecords => settings.ExperimentContext?.Blocks[settings.Block].TrialRecords ?? new ObservableCollection<StroopTrial>();

        public ExperimentProgressViewModel(ExperimentSettings settings)
        {
            this.settings = settings;
            if (this.settings.ExperimentContext != null)
            {
                this.settings.ExperimentContext.PropertyChanged += ExperimentContext_PropertyChanged;
            }
        }

        private void ExperimentContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Progress));
            OnPropertyChanged(nameof(TrialRecords));
        }
    }
}
