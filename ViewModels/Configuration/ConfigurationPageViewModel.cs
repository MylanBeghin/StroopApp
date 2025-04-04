using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using ModernWpf.Controls;
using StroopApp.Commands;
using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.ViewModels.Configuration.Participant;
using StroopApp.ViewModels.Configuration.Profile;
using StroopApp.Views;

namespace StroopApp.ViewModels.Configuration
{
    public class ConfigurationPageViewModel
    {
        private readonly ProfileManagementViewModel _profileViewModel;
        private readonly ParticipantManagementViewModel _participantViewModel;
        private readonly KeyMappingViewModel _keyMappingViewModel;

        public ExperimentSettings _settings { get; set; }
        public ICommand LaunchExperimentCommand { get; }

        private readonly INavigationService _navigationService;

        public ConfigurationPageViewModel(ProfileManagementViewModel profileViewModel,
                                          ParticipantManagementViewModel participantViewModel,
                                          KeyMappingViewModel keyMappingViewModel,
                                          INavigationService navigationService
                                          )
        {
            _profileViewModel = profileViewModel;
            _participantViewModel = participantViewModel;
            _keyMappingViewModel = keyMappingViewModel;
            _navigationService = navigationService;
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
                await ShowErrorDialog("Veuillez sélectionner un profil d'expérience.");
                return;
            }

            if (_settings.Participant == null)
            {
                await ShowErrorDialog("Veuillez sélectionner un participant.");
                return;
            }
            _settings.ExperimentContext = new SharedExperimentData(_settings);
            _navigationService.NavigateTo<ExperimentDashBoardPage>(_settings);
            var participantWindow = new ParticipantWindow(_settings);
            participantWindow.Show();
        }

        private async Task ShowErrorDialog(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Erreur",
                Content = message,
                CloseButtonText = "OK"
            };
            await dialog.ShowAsync();
        }
    }
}
