using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
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
        private readonly IExportationService _exportationService;
        private readonly INavigationService _experimenterNavigationService;
        private readonly IWindowManager _windowManager;

        public ICommand ContinueCommand { get; }
        public ICommand RestartCommand { get; }
        public ICommand QuitCommand { get; }
        public EndExperimentViewModel(ExperimentSettings settings, IExportationService exportationService, INavigationService experimenterNavigationService, IWindowManager windowManager)
        {
            Settings = settings;
            _exportationService = exportationService;
            _experimenterNavigationService = experimenterNavigationService;
            _windowManager = windowManager;
            ContinueCommand = new RelayCommand(Continue);
            QuitCommand = new RelayCommand(Quit);
            RestartCommand = new RelayCommand(Restart);
        }
        private void Continue()
        {
            Settings.NewBlock();
            _experimenterNavigationService.NavigateTo(() => new ConfigurationPage(Settings, _experimenterNavigationService, _windowManager));
        }
        private void Quit()
        {
            _exportationService.ExportDataAsync();
        }
        private void Restart()
        {
            _exportationService.ExportDataAsync();
        }
    }
}
