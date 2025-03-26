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

        public ExperimentSettings Settings { get; set; }
        public ICommand LaunchExperimentCommand { get; }

        private readonly INavigationService _navigationService;

        public ConfigurationPageViewModel(ProfileManagementViewModel profileViewModel,
                                          ParticipantManagementViewModel participantViewModel,
                                          KeyMappingViewModel keyMappingViewModel,
                                          INavigationService navigationService)
        {
            _profileViewModel = profileViewModel;
            _participantViewModel = participantViewModel;
            _keyMappingViewModel = keyMappingViewModel;
            _navigationService = navigationService;

            Settings = new ExperimentSettings();
            LaunchExperimentCommand = new RelayCommand(LaunchExperiment);
        }


        private async void LaunchExperiment()
        {
            Settings.CurrentProfile = _profileViewModel.CurrentProfile;
            Settings.Participant = _participantViewModel.SelectedParticipant;
            Settings.KeyMappings = _keyMappingViewModel.Mappings;

            if (Settings.CurrentProfile == null)
            {
                await ShowErrorDialog("Veuillez sélectionner un profil d'expérience.");
                return;
            }

            if (Settings.Participant == null)
            {
                await ShowErrorDialog("Veuillez sélectionner un participant.");
                return;
            }
            _navigationService.NavigateTo<ExperimentDashBoardPage>(Settings);

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
