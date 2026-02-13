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
using StroopApp.Views.Configuration;
using StroopApp.Views.KeyMapping;
using StroopApp.Views.Participant;
using StroopApp.Views.Profile;
using System.Windows.Controls;

namespace StroopApp.Views
{
    public partial class ConfigurationPage : Page
    {
        /// <summary>
        /// Initializes ConfigurationPage with all dependencies injected via DI container.
        /// </summary>
        public ConfigurationPage(
            ExperimentSettings settings,
            INavigationService experimentNavigationService,
            IWindowManager windowManager,
            ILanguageService languageService,
            IProfileService profileService,
            IParticipantService participantService,
            IKeyMappingService keyMappingService,
            IExportationService exportationService,
            ITrialGenerationService trialGenerationService)
        {
            InitializeComponent();

            var profileViewModel = new ProfileManagementViewModel(profileService);
            var participantViewModel = new ParticipantManagementViewModel(participantService, settings.ExperimentContext.IsParticipantSelectionEnabled);
            var keyMappingViewModel = new KeyMappingViewModel(keyMappingService);
            var exportFolderSelectorViewModel = new ExportFolderSelectorViewModel(settings, exportationService);

            DataContext = new ConfigurationPageViewModel(settings, profileViewModel, participantViewModel, keyMappingViewModel, experimentNavigationService, windowManager, trialGenerationService, languageService);

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
