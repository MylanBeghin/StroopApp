using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
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
        private readonly IExportationService _exportationService;
        private readonly INavigationService _experimenterNavigationService;
        private readonly IWindowManager _windowManager;

        public ICommand ContinueCommand { get; }
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
        public EndExperimentViewModel(ExperimentSettings settings, IExportationService exportationService, INavigationService experimenterNavigationService, IWindowManager windowManager)
        {
            Settings = settings;
            _exportationService = exportationService;
            _experimenterNavigationService = experimenterNavigationService;
            _windowManager = windowManager;
            ContinueCommand = new RelayCommand(Continue);
            QuitCommand = new RelayCommand(Quit);
        }

        private void UpdateTime()
        {
            CurrentDay = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            CurrentTime = DateTime.Now.ToString("HH:mm");
        }

        private void Continue()
        {
            Settings.NewBlock();
            _experimenterNavigationService.NavigateTo(() => new ConfigurationPage(Settings, _experimenterNavigationService, _windowManager));
        }
        private async void Quit()
        {
            await _exportationService.ExportDataAsync();
            Application.Current.Shutdown();
        }
    }
}
