using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Views;
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
        public EndExperimentViewModel(ExperimentSettings settings, IExportationService exportationService)
        {
            Settings = settings;
            ExportationService = exportationService;
            ContinueCommand = new RelayCommand(Continue);
            QuitCommand = new RelayCommand(Quit);
            RestartCommand = new RelayCommand(Restart);
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
