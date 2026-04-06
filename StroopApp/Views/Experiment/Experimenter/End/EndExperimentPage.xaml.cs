using StroopApp.Services.Exportation;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.ViewModels.Experiment.Experimenter.End;
using StroopApp.ViewModels.State;
using System.Windows.Controls;

namespace StroopApp.Views.Experiment.Experimenter
{
    public partial class EndExperimentPage : Page, INavigationAware
    {
        private readonly ExperimentSettingsViewModel _settings;
        private readonly IExportationService _exportationService;
        private readonly IWindowManager _windowManager;

        INavigationService INavigationAware.NavigationService
        {
            set => Initialize(value);
        }

        public EndExperimentPage(ExperimentSettingsViewModel settings, IExportationService exportationService, IWindowManager windowManager)
        {
            InitializeComponent();
            _settings = settings;
            _exportationService = exportationService;
            _windowManager = windowManager;
            Unloaded += (s, e) => (DataContext as IDisposable)?.Dispose();
        }

        private void Initialize(INavigationService navigationService)
        {
            DataContext = new EndExperimentPageViewModel(_settings, _exportationService, navigationService, _windowManager);
        }
    }
}