using StroopApp.Models;
using StroopApp.Services.Exportation;
using StroopApp.Services.KeyMapping;
using StroopApp.Services.Language;
using StroopApp.Services.Navigation;
using StroopApp.Services.Participant;
using StroopApp.Services.Profile;
using StroopApp.Services.Trial;
using StroopApp.Services.Window;
using StroopApp.ViewModels.Configuration;
using StroopApp.ViewModels.Configuration.Participant;
using StroopApp.ViewModels.Configuration.Profile;
using StroopApp.ViewModels.State;
using StroopApp.Views.Configuration;
using StroopApp.Views.KeyMapping;
using StroopApp.Views.Participant;
using StroopApp.Views.Profile;
using System.Windows.Controls;

namespace StroopApp.Views
{
    public partial class ConfigurationPage : Page, INavigationAware
    {
        private readonly ExperimentSettingsViewModel _settings;
        private readonly IWindowManager _windowManager;
        private readonly ILanguageService _languageService;
        private readonly IProfileService _profileService;
        private readonly IParticipantService _participantService;
        private readonly IKeyMappingService _keyMappingService;
        private readonly IExportationService _exportationService;
        private readonly ITrialGenerationService _trialGenerationService;

        public INavigationService NavigationService
        {
            set => Initialize(value);
        }

        public ConfigurationPage(
            ExperimentSettingsViewModel settings,
            IWindowManager windowManager,
            ILanguageService languageService,
            IProfileService profileService,
            IParticipantService participantService,
            IKeyMappingService keyMappingService,
            IExportationService exportationService,
            ITrialGenerationService trialGenerationService)
        {
            InitializeComponent();
            _settings = settings;
            _windowManager = windowManager;
            _languageService = languageService;
            _profileService = profileService;
            _participantService = participantService;
            _keyMappingService = keyMappingService;
            _exportationService = exportationService;
            _trialGenerationService = trialGenerationService;
        }

        private void Initialize(INavigationService navigationService)
        {
            var profileViewModel = new ProfileManagementViewModel(_profileService);
            var participantViewModel = new ParticipantManagementViewModel(_participantService, _settings.ExperimentContext.IsParticipantSelectionEnabled);
            var keyMappingViewModel = new KeyMappingViewModel(_keyMappingService);
            var exportFolderSelectorViewModel = new ExportFolderSelectorViewModel(_settings, _exportationService);

            DataContext = new ConfigurationPageViewModel(_settings, profileViewModel, participantViewModel, keyMappingViewModel, navigationService, _windowManager, _trialGenerationService, _languageService);

            var profileManagementView = new ProfileManagementView(profileViewModel);
            var participantManagementView = new ParticipantManagementView(participantViewModel);
            var keyMappingView = new KeyMappingView(keyMappingViewModel);
            var exportFolderView = new ExportFolderSelectorView(exportFolderSelectorViewModel);

            MainGrid.Children.Add(profileManagementView);
            Grid.SetRow(profileManagementView, 1);
            KeyMappingContainer.Children.Add(keyMappingView);
            Grid.SetColumn(keyMappingView, 0);
            KeyMappingContainer.Children.Add(exportFolderView);
            Grid.SetColumn(exportFolderView, 2);
            MainGrid.Children.Add(participantManagementView);
            Grid.SetRow(participantManagementView, 5);
        }
    }
}
