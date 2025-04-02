using StroopApp.Services.Navigation;
using StroopApp.Models;
using System.Runtime.CompilerServices;
using System.ComponentModel;
namespace StroopApp.ViewModels.Experiment
{
    public class ExperimentDashBoardPageViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService NavigationService;
        public ExperimentSettings _settings { get; set; }

        public int CurrentBlock {get;set;}

        private int _currentTrial;
        public int CurrentTrial
        {
            get => _currentTrial;
            set
            {
                if (value != _currentTrial)
                {
                    _currentTrial = value;
                    OnPropertyChanged();
                }
            }
        }
        private double _progress;
        public double Progress
        {
            get => _progress;
            set
            {
                if (value != _progress)
                {
                    _progress = value;
                    OnPropertyChanged();
                }
            }
        }
        public ExperimentDashBoardPageViewModel(INavigationService navigationService, ExperimentSettings settings)
        {
            NavigationService = navigationService;
            _settings = settings;
            CurrentTrial = 0;
            Progress = 0;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
