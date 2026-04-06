using StroopApp.Services.Language;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.ViewModels.Experiment.Experimenter;
using StroopApp.ViewModels.State;
using System.Windows.Controls;

namespace StroopApp.Views
{
    public partial class ExperimentDashBoardPage : Page, INavigationAware
    {
        private readonly ExperimentSettingsViewModel _settings;
        private readonly IWindowManager _windowManager;
        private readonly ILanguageService _languageService;

        public INavigationService NavigationService
        {
            set => Initialize(value);
        }

        public ExperimentDashBoardPage(ExperimentSettingsViewModel settings, IWindowManager windowManager, ILanguageService languageService)
        {
            InitializeComponent();
            _settings = settings;
            _windowManager = windowManager;
            _languageService = languageService;
            Unloaded += (s, e) => (DataContext as IDisposable)?.Dispose();
        }

        private void Initialize(INavigationService navigationService)
        {
            DataContext = new ExperimentDashBoardPageViewModel(_settings, navigationService, _windowManager, _languageService);
        }
    }
}
