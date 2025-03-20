using System.Windows.Input;
using ModernWpf.Controls;
using StroopApp.Commands;
using StroopApp.Views.Participant;
using StroopApp.Views.Profile;
using StroopApp.Views.KeyMapping;
using StroopApp.Models;
using StroopApp.Views;

namespace StroopApp.ViewModels
{
    public class ConfigurationPageViewModel
    {
        private readonly ProfileManagementView _profileManagementView;
        private readonly ParticipantManagementView _participantManagementView;
        private readonly KeyMappingView _keyMappingView;
        public ExperimentSettings Settings { get; set; }
        public ICommand LaunchExperimentCommand { get; }

        public ConfigurationPageViewModel(ProfileManagementView profileManagementView,
                                          ParticipantManagementView participantManagementView,
                                          KeyMappingView keyMappingView)
        {
            _profileManagementView = profileManagementView;
            _participantManagementView = participantManagementView;
            _keyMappingView = keyMappingView;
            Settings = new ExperimentSettings();
            LaunchExperimentCommand = new RelayCommand(LaunchExperiment);
        }

        private async void LaunchExperiment()
        {
            var profileVM = (ProfileManagementViewModel)_profileManagementView.DataContext;
            var participantVM = (ParticipantManagementViewModel)_participantManagementView.DataContext;
            var keyMappingVM = (KeyMappingViewModel)_keyMappingView.DataContext;
            Settings.CurrentProfile = profileVM.CurrentProfile;
            Settings.Participant = participantVM.SelectedParticipant;
            Settings.KeyMappings = keyMappingVM.Mappings;

            if (Settings.CurrentProfile == null)
            {
                var dialog = new ContentDialog
                {
                    Title = "Erreur",
                    Content = "Veuillez sélectionner un profil d'expérience.",
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
                return;
            }
            if (Settings.Participant == null)
            {
                var dialog = new ContentDialog
                {
                    Title = "Erreur",
                    Content = "Veuillez sélectionner un participant.",
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
                return;
            }

            var dialogtest = new ContentDialog
            {
                Title = "Erreur",
                Content = "YES MON GARS",
                CloseButtonText = "OK"
            };
            await dialogtest.ShowAsync();
            return;
        }
    }
}
