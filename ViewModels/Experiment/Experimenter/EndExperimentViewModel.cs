using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StroopApp.ViewModels.Experiment.Experimenter
{
    public class EndExperimentViewModel : ViewModelBase
    {
        private ExperimentSettings _settings;
        public ExperimentSettings Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                OnPropertyChanged();
            }
        }
        public IExportationService ExportationService { get; }
        public ICommand ContinueCommand { get; }
        public ICommand RestartCommand { get; }
        public ICommand QuitCommand { get; }

        private string _currentDay;
        public string CurrentDay
        {
            get => _currentDay;
            set
            {
                _currentDay = value;
                OnPropertyChanged();
            }
        }

        private string _currentTime;
        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Block> Blocks => Settings.ExperimentContext.Blocks;

        public EndExperimentViewModel(ExperimentSettings settings, IExportationService svc)
        {
            Settings = settings;
            ExportationService = svc;
            ContinueCommand = new RelayCommand(Continue);
            RestartCommand = new RelayCommand(Restart);
            QuitCommand = new RelayCommand(Quit);
            UpdateTime();
            Settings.ExperimentContext.AddCurrentBlock(settings);
        }

        private void UpdateTime()
        {
            CurrentDay = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            CurrentTime = DateTime.Now.ToString("HH:mm");
        }

        private void Continue()
        {
            App.ExperimentWindowNavigationService.NavigateTo(() => new ConfigurationPage());
        }
        private void Quit()
        {
            ExportationService.ExportDataAsync();
        }
        private void Restart()
        {
            ExportationService.ExportDataAsync();
        }
    }
}
