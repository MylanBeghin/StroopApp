using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace StroopApp.ViewModels.Experiment.Experimenter
{
    public class EndExperimentViewModel : ViewModelBase
    {
        public ExperimentSettings Settings { get; }
        public ObservableCollection<Block> Blocks { get; }
        private readonly IExportationService _exportationService;
        private readonly INavigationService _experimenterNavigationService;
        private readonly IWindowManager _windowManager;

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
        public EndExperimentViewModel(ExperimentSettings settings,
                                  IExportationService exportationService,
                                  INavigationService experimenterNavigationService,
                                  IWindowManager windowManager)
        {
            Settings = settings;
            _exportationService = exportationService;
            _experimenterNavigationService = experimenterNavigationService;
            _windowManager = windowManager;
            ContinueCommand = new RelayCommand(Continue);
            RestartCommand = new RelayCommand(Restart);
            QuitCommand = new RelayCommand(Quit);

            UpdateTime();
            Settings.NewBlock();
            Blocks = Settings.ExperimentContext.Blocks;
        }

        private void UpdateTime()
        {
            CurrentDay = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            CurrentTime = DateTime.Now.ToString("HH:mm");
        }

        private void Continue()
        {
            _experimenterNavigationService.NavigateTo(() => new ConfigurationPage(Settings, _experimenterNavigationService, _windowManager));
        }
        private async void Restart()
        {
            await _exportationService.ExportDataAsync();
            _experimenterNavigationService.NavigateTo(() => new ConfigurationPage(new ExperimentSettings(), _experimenterNavigationService, _windowManager));
        }
        private async void Quit()
        {
            await _exportationService.ExportDataAsync();
            Application.Current.Shutdown();
        }
    }
}
