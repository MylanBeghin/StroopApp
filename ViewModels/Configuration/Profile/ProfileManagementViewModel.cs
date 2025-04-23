using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ModernWpf.Controls;
using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services;
using StroopApp.Views;

namespace StroopApp.ViewModels.Configuration.Profile
{
    public class ProfileManagementViewModel : INotifyPropertyChanged
    {
        private readonly IProfileService _profileService;
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
                        _profileService.SaveLastSelectedProfile(_currentProfile);
                }
            }
        }

        public ICommand CreateProfileCommand { get; }
        public ICommand ModifyProfileCommand { get; }
        public ICommand DeleteProfileCommand { get; }

        public ProfileManagementViewModel(IProfileService profileService)
        {
            _profileService = profileService;
            Profiles = _profileService.LoadProfiles();

            var lastProfileName = _profileService.LoadLastSelectedProfile();
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
            var viewModel = new ProfileEditorViewModel(newProfile, Profiles, _profileService);
            var profileWindow = new ProfileEditorWindow(viewModel);
            profileWindow.ShowDialog();
            if (profileWindow.DialogResult == true)
            {
                Profiles.Add(newProfile);
                _profileService.SaveProfiles(Profiles);
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
            var viewModel = new ProfileEditorViewModel(CurrentProfile, Profiles, _profileService);
            var profileWindow = new ProfileEditorWindow(viewModel);
            profileWindow.ShowDialog();
            if (profileWindow.DialogResult == true)
            {
                OnPropertyChanged(nameof(CurrentProfile));
                _profileService.SaveProfiles(Profiles);
            }
        }


        private void DeleteProfile()
        {
            if (CurrentProfile == null)
            {
                ShowErrorDialog("Veuillez sélectionner un profil à supprimer !");
                return;
            }
            int currentIndex = Profiles.IndexOf(_currentProfile);
            _profileService.DeleteProfile(_currentProfile, Profiles);

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
