using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
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
            var profilesWindow = new ProfileEditorWindow(newProfile, Profiles, _IprofileService);
            profilesWindow.ShowDialog();
            CurrentProfile = newProfile;
        }
        private void ModifyProfile()
        {
            if (_currentProfile == null)
            {
                MessageBox.Show("Veuillez sélectionner un profil à modifier !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var profilesWindow = new ProfileEditorWindow(_currentProfile, Profiles, _IprofileService);
            profilesWindow.ShowDialog();
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
                if (currentIndex >= Profiles.Count)
                {
                    currentIndex = Profiles.Count - 1;
                }
                CurrentProfile = Profiles[currentIndex];
            }
            else
            {
                CurrentProfile = null;
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
