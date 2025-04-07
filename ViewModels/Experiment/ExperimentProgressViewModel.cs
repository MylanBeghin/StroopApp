using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StroopApp.Models;

namespace StroopApp.ViewModels.Experiment
{
    public class ExperimentProgressViewModel : INotifyPropertyChanged
    {
        private readonly ExperimentSettings _settings;
        public int CurrentTrialNumber => _settings.ExperimentContext?.CurrentTrial?.TrialNumber ?? 0;
        public int TotalTrials => _settings.ExperimentContext?.TotalTrials ?? 0;
        public double Progress => (int)(_settings.ExperimentContext?.Progress * 100 ?? 0) ;
        public ObservableCollection<StroopTrial> TrialRecords => _settings.ExperimentContext?.TrialRecords ?? new ObservableCollection<StroopTrial>();
        public ExperimentProgressViewModel(ExperimentSettings settings)
        {
            _settings = settings;
            _settings.ExperimentContext.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SharedExperimentData.CurrentTrial) ||
                    e.PropertyName == nameof(SharedExperimentData.TrialRecords) ||
                    e.PropertyName == nameof(SharedExperimentData.Progress))
                {
                    OnPropertyChanged(nameof(CurrentTrialNumber));
                    OnPropertyChanged(nameof(TrialRecords));
                    OnPropertyChanged(nameof(Progress));
                }
            };
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
