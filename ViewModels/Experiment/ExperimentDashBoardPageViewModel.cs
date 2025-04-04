using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StroopApp.Models;
using StroopApp.Services.Navigation;

namespace StroopApp.ViewModels.Experiment
{
    public class ExperimentDashBoardPageViewModel : INotifyPropertyChanged
    {
        private readonly ExperimentSettings _settings;
        private readonly SharedExperimentData _experimentContext;

        public ExperimentDashBoardPageViewModel(INavigationService navigationService, ExperimentSettings settings)
        {
            _settings = settings;
            _experimentContext = settings.ExperimentContext;
            _experimentContext.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SharedExperimentData.CurrentTrialNumber) ||
                    e.PropertyName == nameof(SharedExperimentData.TrialRecords))
                {
                    OnPropertyChanged(nameof(CurrentTrialNumber));
                    OnPropertyChanged(nameof(TrialRecords));
                }
            };
        }

        public int CurrentTrialNumber => _experimentContext.CurrentTrialNumber;
        public int TotalTrials => _experimentContext.TotalTrials;
        public ObservableCollection<StroopTrial> TrialRecords => _experimentContext.TrialRecords;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
