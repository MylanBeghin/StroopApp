using System.Threading.Tasks;
using System.Windows.Input;
using ModernWpf.Controls;
using StroopApp.Commands;
using StroopApp.Models;

namespace StroopApp.ViewModels
{
    public class ConfigurationPageViewModel
    {
        private readonly ProfileManagementViewModel _profileViewModel;
        private readonly ParticipantManagementViewModel _participantViewModel;
        private readonly KeyMappingViewModel _keyMappingViewModel;

        public ExperimentSettings Settings { get; set; }
        public ICommand LaunchExperimentCommand { get; }

        public ConfigurationPageViewModel(ProfileManagementViewModel profileViewModel,
                                          ParticipantManagementViewModel participantViewModel,
                                          KeyMappingViewModel keyMappingViewModel)
        {
            _profileViewModel = profileViewModel;
            _participantViewModel = participantViewModel;
            _keyMappingViewModel = keyMappingViewModel;

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

            await ShowSuccessDialog("L'expérience va démarrer !");
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

        private async Task ShowSuccessDialog(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Succès",
                Content = message,
                CloseButtonText = "OK"
            };
            await dialog.ShowAsync();
        }
    }
}
