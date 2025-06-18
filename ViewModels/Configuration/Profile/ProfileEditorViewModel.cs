using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace StroopApp.ViewModels.Configuration.Profile
{
    public class ProfileEditorViewModel : ViewModelBase
    {
        private ExperimentProfile _profile;

        public ExperimentProfile Profile
        {
            get => _profile;
            set
            {
                _profile = value;
                OnPropertyChanged();
            }
        }

        public ExperimentProfile ModifiedProfile => Profile;

        public ObservableCollection<ExperimentProfile> Profiles
        {
            get;
        }

        public bool? DialogResult
        {
            get; private set;
        }

        public Action? CloseAction
        {
            get; set;
        }

        public ICommand SaveCommand
        {
            get;
        }

        public ICommand CancelCommand
        {
            get;
        }

        private readonly IProfileService _IprofileService;

        public ProfileEditorViewModel(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles, IProfileService profileService)
        {
            _IprofileService = profileService;
            Profiles = profiles;

            Profile = Profiles.Contains(profile) ? CloneProfile(profile) : profile;
            Profile.UpdateDerivedValues();

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);

            Profile.PropertyChanged += Profile_PropertyChanged;
        }

        private void Profile_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Profile.WordCount) ||
                e.PropertyName == nameof(Profile.WordDuration) ||
                e.PropertyName == nameof(Profile.Hours) ||
                e.PropertyName == nameof(Profile.Minutes) ||
                e.PropertyName == nameof(Profile.Seconds) ||
                e.PropertyName == nameof(Profile.CalculationMode))
            {
                Profile.UpdateDerivedValues();
            }
            else if (e.PropertyName == nameof(Profile.IsAmorce))
            {
                if (Profile.IsAmorce)
                {
                    Profile.AmorceDuration = 0;
                }
            }
        }

        private ExperimentProfile CloneProfile(ExperimentProfile profile)
        {
            return new ExperimentProfile
            {
                Id = profile.Id,
                ProfileName = profile.ProfileName,
                CalculationMode = profile.CalculationMode,
                Hours = profile.Hours,
                Minutes = profile.Minutes,
                Seconds = profile.Seconds,
                TaskDuration = profile.TaskDuration,
                WordDuration = profile.WordDuration,
                MaxReactionTime = profile.MaxReactionTime,
                GroupSize = profile.GroupSize,
                AmorceDuration = profile.AmorceDuration,
                FixationDuration = profile.FixationDuration,
                WordCount = profile.WordCount,
                IsAmorce = profile.IsAmorce,
                SwitchPourcentage = profile.SwitchPourcentage,
                CongruencePourcentage = profile.CongruencePourcentage
            };
        }

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(Profile.ProfileName))
            {
                var loc = App.Current.Resources["Loc"] as StroopApp.Core.LocalizedStrings;
                ShowErrorDialog(loc?["Error_ProfileNameEmpty"] ?? "");
                return;
            }

            if (Profiles.Any(p => p.Id != Profile.Id && p.ProfileName == Profile.ProfileName))
            {
                var loc = App.Current.Resources["Loc"] as StroopApp.Core.LocalizedStrings;
                ShowErrorDialog(loc?["Error_ProfileNameExists"] ?? "");
                return;
            }

            if (Profile.WordDuration <= 0 || Profile.TaskDuration % Profile.WordDuration != 0)
            {
                var loc = App.Current.Resources["Loc"] as StroopApp.Core.LocalizedStrings;
                ShowErrorDialog(loc?["Error_WordDurationInvalid"] ?? "");
                return;
            }

            int wordNumber = Profile.TaskDuration / Profile.WordDuration;
            if (Profile.GroupSize <= 0 || wordNumber % Profile.GroupSize != 0)
            {
                var loc = App.Current.Resources["Loc"] as StroopApp.Core.LocalizedStrings;
                ShowErrorDialog(loc?["Error_GroupSizeInvalid"] ?? "");
                return;
            }

            if (Profile.AmorceDuration == 0 && Profile.IsAmorce)
            {
                var loc = App.Current.Resources["Loc"] as StroopApp.Core.LocalizedStrings;
                ShowErrorDialog(loc?["Error_AmorceTimeInvalid"] ?? "");
                return;
            }

            if (Profile.MaxReactionTime <= 0)
            {
                var loc = App.Current.Resources["Loc"] as StroopApp.Core.LocalizedStrings;
                ShowErrorDialog(loc?["Error_MaxReactionInvalid"] ?? "");
                return;
            }

            DialogResult = true;
            CloseAction?.Invoke();
        }

        public void Cancel()
        {
            DialogResult = false;
            CloseAction?.Invoke();
        }
    }
}
