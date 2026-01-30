using DocumentFormat.OpenXml.Wordprocessing;
using StroopApp.Core;
using StroopApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StroopApp.ViewModels.Experiment.Experimenter
{
    /// <summary>
    /// ViewModel for tracking experiment progress, displaying completion percentage and current trial records.
    /// Updates automatically when experiment context changes.
    /// </summary>
    public class ExperimentProgressViewModel : ViewModelBase, IDisposable
    {
        public ExperimentSettings Settings
        {
            get;
        }
        public int Progress => (Settings.CurrentProfile.WordCount > 0) && (Settings.ExperimentContext != null)
    ? (int)(((double)Settings.ExperimentContext.ReactionPoints.Count / Settings.CurrentProfile.WordCount) * 100)
        : 0;

        public ObservableCollection<StroopTrial> TrialRecords =>
    Settings.ExperimentContext?.Blocks.Count > Settings.Block
        ? Settings.ExperimentContext.Blocks[Settings.Block].TrialRecords
        : new ObservableCollection<StroopTrial>();

        public ExperimentProgressViewModel(ExperimentSettings settings)
        {
            this.Settings = settings;
            if (this.Settings.ExperimentContext != null)
            {
                this.Settings.ExperimentContext.PropertyChanged += ExperimentContext_PropertyChanged;
            }
        }

        private void ExperimentContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Progress));
            OnPropertyChanged(nameof(TrialRecords));
        }
        public void Dispose()
        {
            if (Settings.ExperimentContext != null)
            {
                Settings.ExperimentContext.PropertyChanged -= ExperimentContext_PropertyChanged;
            }
        }
    }
}
