using StroopApp.Core;
using StroopApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StroopApp.ViewModels.Experiment
{
    public class ExperimentProgressViewModel : ViewModelBase
    {
        private readonly ExperimentSettings _settings;
        public ExperimentSettings Settings => _settings;

        public int Progress => (_settings.CurrentProfile.WordCount > 0) && (_settings.ExperimentContext != null)
    ? (int)(((double)_settings.ExperimentContext.ReactionPoints.Count / _settings.CurrentProfile.WordCount) * 100)
    : 0;

        public ObservableCollection<StroopTrial> TrialRecords => _settings.ExperimentContext?.TrialRecords ?? new ObservableCollection<StroopTrial>();

        public ExperimentProgressViewModel(ExperimentSettings settings)
        {
            _settings = settings;
            if (_settings.ExperimentContext != null)
            {
                _settings.ExperimentContext.PropertyChanged += ExperimentContext_PropertyChanged;
            }
        }

        private void ExperimentContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Progress));
            OnPropertyChanged(nameof(TrialRecords));
        }
    }
}
