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
        public ExperimentSettings Settings => _settings;

        public int CurrentTrialNumber => _settings.ExperimentContext?.CurrentTrial?.TrialNumber ?? 0;
        public int TotalTrials => _settings.ExperimentContext?.TotalTrials ?? 0;
        public double Progress => (int)((_settings.ExperimentContext?.Progress ?? 0) * 100);
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
            OnPropertyChanged(nameof(CurrentTrialNumber));
            OnPropertyChanged(nameof(TotalTrials));
            OnPropertyChanged(nameof(Progress));
            OnPropertyChanged(nameof(TrialRecords));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
