using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ModernWpf.Controls;
using StroopApp.Commands;
using StroopApp.Models;
using StroopApp.Services;
using StroopApp.Services.Profile;
using StroopApp.Views;

namespace StroopApp.ViewModels
{
    public class ProfileManagementViewModel : INotifyPropertyChanged
    {
        private readonly IProfileService _IprofileService;
        public ObservableCollection<ExperimentProfile> Profiles { get; set; }

        private ExperimentProfile? _currentProfile;
        public ExperimentProfile CurrentProfile
        {
            get => _currentProfile;
            set
            {
                if (_currentProfile != value)
                {
                    _currentProfile = value;
                    OnPropertyChanged();
                    if (_currentProfile != null)
                        _IprofileService.SaveLastSelectedProfile(_currentProfile);
                }
            }
        }
        public ICommand CreateProfileCommand { get; }
        public ICommand ModifyProfileCommand { get; }
        public ICommand DeleteProfileCommand { get; }
        public ProfileManagementViewModel(IProfileService profileService)
        {
            _IprofileService = profileService;
            Profiles = _IprofileService.LoadProfiles();
            // Charger le dernier profil enregistré
            var lastProfileName = _IprofileService.LoadLastSelectedProfile();
            if (!string.IsNullOrEmpty(lastProfileName))
            {
                CurrentProfile = Profiles.FirstOrDefault(p => p.ProfileName == lastProfileName);
            }
            CreateProfileCommand = new RelayCommand(CreateProfile);
            ModifyProfileCommand = new RelayCommand(ModifyProfile);
            DeleteProfileCommand = new RelayCommand(DeleteProfile);
        }

        private void CreateProfile()
        {
            var newProfile = new ExperimentProfile();
            var profileWindow = new ProfileEditorWindow(newProfile, Profiles, _IprofileService);
            profileWindow.ShowDialog();
            if (profileWindow.DialogResult == true)
            {
                Profiles.Add(newProfile);
                _IprofileService.SaveProfiles(Profiles);
                CurrentProfile = newProfile;
            }
        }
        private void ModifyProfile()
        {
            if (CurrentProfile == null)
            {
                ShowErrorDialog("Veuillez sélectionner un profil à modifier !");
                return;
            }

            var profileEditorViewModel = new ProfileEditorViewModel(CurrentProfile, Profiles, _IprofileService);
            var profileEditorWindow = new ProfileEditorWindow(CurrentProfile, Profiles, _IprofileService);
            profileEditorWindow.ShowDialog();
            if (profileEditorWindow.DialogResult == true)
            {
                CurrentProfile = profileEditorViewModel.Profile;
                _IprofileService.SaveProfiles(Profiles);
            }
        }
        private void DeleteProfile()
        {
            if (_currentProfile == null)
            {
                MessageBox.Show("Veuillez sélectionner un profil à supprimer !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int currentIndex = Profiles.IndexOf(_currentProfile);
            _IprofileService.DeleteProfile(_currentProfile, Profiles);

            if (Profiles.Count > 0)
            {
                CurrentProfile = Profiles[0];
            }
            else
            {
                CurrentProfile = null;
            }
        }
        private async void ShowErrorDialog(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Erreur",
                Content = message,
                CloseButtonText = "OK"
            };

            await dialog.ShowAsync();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
