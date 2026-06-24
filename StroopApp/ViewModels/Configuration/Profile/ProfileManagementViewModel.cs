using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.Services.Navigation;
using StroopApp.Services.Profile;
using StroopApp.Views;
using StroopApp.Views.Configuration.Profile;
using System.Collections.ObjectModel;
using System.Windows;

namespace StroopApp.ViewModels.Configuration.Profile
{
    /// <summary>
    /// ViewModel for managing experiment profiles with create, edit, and delete operations.
    /// Automatically persists the last selected profile for restoration on next launch.
    /// </summary>
    public partial class ProfileManagementViewModel : ViewModelBase
    {
        private readonly IProfileService _profileService;

        public ObservableCollection<ExperimentProfile> Profiles { get; }

        [ObservableProperty]
        private ExperimentProfile? _currentProfile;
        private TaskType _taskType;

        partial void OnCurrentProfileChanged(ExperimentProfile? value)
        {
            if (value != null)
                _profileService.SaveLastSelectedProfile(value);
        }

        public ProfileManagementViewModel(IProfileService profileService, TaskType taskType)
        {
            _profileService = profileService;
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
                var newProfile = new ExperimentProfile { TaskType = _taskType };
                var viewModel = OpenProfileEditor(newProfile);

                if (viewModel is not null)
                {
                    var updatedProfiles = _profileService.UpsertProfile(viewModel.ModifiedProfile);
                    Profiles.Clear();
                    foreach (var prof in updatedProfiles.Where(p => p.TaskType == _taskType))
                        Profiles.Add(prof);
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
                var viewModel = OpenProfileEditor(CurrentProfile);
                if (viewModel is not null)
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

        private ProfileEditorViewModelBase? OpenProfileEditor(ExperimentProfile profile)
        {
            ProfileEditorViewModelBase vm;
            Window win;
            if (_taskType == TaskType.Stroop)
            {
                vm = new StroopProfileEditorViewModel(profile, Profiles, _profileService);
                win = new StroopProfileEditorWindow((StroopProfileEditorViewModel)vm);
            }
            else if (_taskType == TaskType.Simon)
            {
                vm = new SimonProfileEditorViewModel(profile, Profiles, _profileService);
                win = new SimonProfileEditorWindow((SimonProfileEditorViewModel)vm);
            }
            else
                return null;

            win.ShowDialog();
            return win.DialogResult == true ? vm : null;
        }
    }
}