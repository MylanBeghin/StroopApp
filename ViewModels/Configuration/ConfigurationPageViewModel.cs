using StroopApp.Core;
using StroopApp.Models;
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

        public ExperimentSettings _settings { get; set; }
        public ICommand LaunchExperimentCommand { get; }

        public ConfigurationPageViewModel(ProfileManagementViewModel profileViewModel,
                                          ParticipantManagementViewModel participantViewModel,
                                          KeyMappingViewModel keyMappingViewModel
                                          )
        {
            _profileViewModel = profileViewModel;
            _participantViewModel = participantViewModel;
            _keyMappingViewModel = keyMappingViewModel;
            _settings = new ExperimentSettings();
            LaunchExperimentCommand = new RelayCommand(LaunchExperiment);
        }


        private async void LaunchExperiment()
        {
            _settings.CurrentProfile = _profileViewModel.CurrentProfile;
            _settings.Participant = _participantViewModel.SelectedParticipant;
            _settings.KeyMappings = _keyMappingViewModel.Mappings;

            if (_settings.CurrentProfile == null)
            {
                ShowErrorDialog("Veuillez sélectionner un profil d'expérience.");
                return;
            }

            if (_settings.Participant == null)
            {
                ShowErrorDialog("Veuillez sélectionner un participant.");
                return;
            }
            _settings.ExperimentContext = new SharedExperimentData(_settings);
            App.ExperimentWindowNavigationService.NavigateTo(() => new ExperimentDashBoardPage(_settings));
            var partWin = new ParticipantWindow(_settings);
            partWin.Show();
        }
    }
}
