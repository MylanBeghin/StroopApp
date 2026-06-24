using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.Services.Navigation;
using StroopApp.Services.Profile;
using StroopApp.Views;
using StroopApp.Views.Configuration;
using System.Collections.ObjectModel;

namespace StroopApp.ViewModels.Configuration.Profile
{
    /// <summary>
    /// ViewModel for managing experiment profiles with create, edit, and delete operations.
    /// Automatically persists the last selected profile for restoration on next launch.
    /// </summary>
    public partial class ProfileManagementViewModel : ViewModelBase
    {
        private readonly IProfileService _profileService;
        private readonly INavigationService _navigationService
            ;

        public ObservableCollection<ExperimentProfile> Profiles { get; }

        [ObservableProperty]
        private ExperimentProfile? _currentProfile;
        private TaskType _taskType;

        partial void OnCurrentProfileChanged(ExperimentProfile? value)
        {
            if (value != null)
                _profileService.SaveLastSelectedProfile(value);
        }

        public ProfileManagementViewModel(IProfileService profileService, INavigationService navigationService,TaskType taskType)
        {
            _profileService = profileService;
            _navigationService = navigationService;
            _taskType = taskType;
            var allProfiles = _profileService.LoadProfiles();
            Profiles = new ObservableCollection<ExperimentProfile>(
                allProfiles.Where(p => p.TaskType == _taskType));
            var lastId = _profileService.LoadLastSelectedProfile();
            if (lastId.HasValue)
                CurrentProfile = Profiles.FirstOrDefault(p => p.Id == lastId.Value);
        }

        [RelayCommand]
        private async Task CreateProfileAsync()
        {
            try
            {
                var newProfile = new ExperimentProfile();
                newProfile.TaskType = _taskType;
                var viewModel = new ProfileEditorViewModel(newProfile, Profiles, _profileService);
                var profileWindow = new ProfileEditorWindow(viewModel);
                profileWindow.ShowDialog();

                if (profileWindow.DialogResult == true)
                {
                    var updatedProfiles = _profileService.UpsertProfile(newProfile);
                    Profiles.Clear();
                    foreach (var prof in updatedProfiles.Where(p => p.TaskType == _taskType))
                    {
                        Profiles.Add(prof);
                    }
                    CurrentProfile = Profiles.FirstOrDefault(p => p.Id == newProfile.Id);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task ModifyProfileAsync()
        {
            try
            {
                if (CurrentProfile == null)
                {
                    await ShowErrorDialogAsync(Strings.Error_SelectProfileToModify);
                    return;
                }

                var viewModel = new ProfileEditorViewModel(CurrentProfile, Profiles, _profileService);
                var profileWindow = new ProfileEditorWindow(viewModel);
                profileWindow.ShowDialog();

                if (profileWindow.DialogResult == true)
                {
                    _profileService.UpsertProfile(viewModel.ModifiedProfile);
                    CurrentProfile = viewModel.ModifiedProfile;
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task DeleteProfileAsync()
        {
            try
            {
                if (CurrentProfile == null)
                {
                    await ShowErrorDialogAsync(Strings.Error_SelectProfileToDelete);
                    return;
                }

                if (await ShowConfirmationDialogAsync(Strings.Title_DeleteConfirmation, Strings.Message_DeleteProfileConfirmation))
                {
                    var profileToDelete = CurrentProfile;
                    if (profileToDelete is null)
                        return;

                    _profileService.DeleteProfile(profileToDelete, Profiles);
                    CurrentProfile = Profiles.Count > 0 ? Profiles[0] : null;
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }
    }
}