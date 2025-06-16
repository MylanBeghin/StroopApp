using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.ViewModels.Configuration.Participant;
using StroopApp.ViewModels.Configuration.Profile;
using StroopApp.Views;
using System.Windows.Input;

namespace StroopApp.ViewModels.Configuration
{
    public class ConfigurationPageViewModel : ViewModelBase
    {
        private readonly ProfileManagementViewModel _profileViewModel;
        private readonly ParticipantManagementViewModel _participantViewModel;
        private readonly KeyMappingViewModel _keyMappingViewModel;
        private readonly INavigationService _experimenterNavigationService;
        private readonly IWindowManager _windowManager;
        public ExperimentSettings _settings
        {
            get; set;
        }
        public ICommand LaunchExperimentCommand
        {
            get;
        }
        public ICommand BrowseExportFolderCommand
        {
            get;
        }
        public ConfigurationPageViewModel(ExperimentSettings settings,
                                  ProfileManagementViewModel profileViewModel,
                                  ParticipantManagementViewModel participantViewModel,
                                  KeyMappingViewModel keyMappingViewModel,
                                  INavigationService experimenterNavigationService,
                                  IWindowManager windowManager
                                  )
        {
            _profileViewModel = profileViewModel;
            _participantViewModel = participantViewModel;
            _keyMappingViewModel = keyMappingViewModel;
            _experimenterNavigationService = experimenterNavigationService;
            _windowManager = windowManager;
            _settings = settings;
            LaunchExperimentCommand = new RelayCommand(LaunchExperiment);
        }


        private void LaunchExperiment()
        {
            _settings.CurrentProfile = _profileViewModel.CurrentProfile;
            _settings.Participant = _participantViewModel.SelectedParticipant;
            _settings.KeyMappings = _keyMappingViewModel.Mappings;

            if (_settings.CurrentProfile == null)
            {
                var loc = App.Current.Resources["Loc"] as StroopApp.Core.LocalizedStrings;
                ShowErrorDialog(loc?["Error_SelectProfile"] ?? "");
                return;
            }

            if (_settings.Participant == null)
            {
                var loc = App.Current.Resources["Loc"] as StroopApp.Core.LocalizedStrings;
                ShowErrorDialog(loc?["Error_SelectParticipant"] ?? "");
                return;
            }

            _settings.ExperimentContext.AddNewSerie(_settings);
            _experimenterNavigationService.NavigateTo(() => new ExperimentDashBoardPage(_settings, _experimenterNavigationService, _windowManager));
            _windowManager.ShowParticipantWindow(_settings);
        }
    }
}
