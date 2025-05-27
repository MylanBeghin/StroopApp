using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services;
using StroopApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StroopApp.ViewModels.Configuration.Profile
{
    public class ProfileManagementViewModel : ViewModelBase
    {
        private readonly IProfileService _profileService;
        public ObservableCollection<ExperimentProfile> Profiles
        {
            get; set;
        }

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
        public ICommand CreateProfileCommand
        {
            get;
        }
        public ICommand ModifyProfileCommand
        {
            get;
        }
        public ICommand DeleteProfileCommand
        {
            get;
        }

        public ProfileManagementViewModel(IProfileService profileService)
        {
            _profileService = profileService;
            Profiles = _profileService.LoadProfiles();

            var lastId = _profileService.LoadLastSelectedProfile();
            if (lastId.HasValue)
                CurrentProfile = Profiles.FirstOrDefault(p => p.Id == lastId.Value);

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
                _profileService.AddProfile(newProfile, Profiles);
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
                _profileService.UpdateProfileById(
                    viewModel.ModifiedProfile,
                    viewModel.ModifiedProfile.Id,
                    Profiles);
                OnPropertyChanged(nameof(CurrentProfile));
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
    }
}
