using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.Services.Profile;
using System.Collections.ObjectModel;

namespace StroopApp.ViewModels.Configuration.Profile
{
    /// <summary>
    /// ViewModel for creating or editing an experiment profile in a modal dialog.
    /// Handles validation, derived value recalculation, and language selection.
    /// </summary>
    public partial class ProfileEditorViewModel : ViewModelBase
    {
        public ExperimentProfile Profile { get; private set; }
        public ExperimentProfile ModifiedProfile => Profile;
        public ObservableCollection<ExperimentProfile> Profiles { get; }
        public bool? DialogResult { get; private set; }
        public Action? CloseAction { get; set; }
        public SwitchSettingsViewModel SwitchSettingsViewModel { get; }

        private readonly IProfileService _profileService;
        private bool _isUpdating;

        public List<LanguageOption> Languages { get; } = new()
        {
            new LanguageOption { Code = "fr", DisplayName = "Français" },
            new LanguageOption { Code = "en", DisplayName = "English" }
        };

        [ObservableProperty]
        private LanguageOption _selectedTaskLanguage;

        partial void OnSelectedTaskLanguageChanged(LanguageOption value)
        {
            if (value != null)
            {
                TaskLanguage = value.Code;
            }
        }

        // Now using ObervsableProperty for automatic INotifyPropertyChanged implementation, instead of manually
        // implementing properties and calling OnPropertyChanged. This reduces boilerplate and potential for errors.
        // N.B : Migration to .NET 10 will allow even more concise code.

        [ObservableProperty]
        private string _profileName = string.Empty;

        [ObservableProperty]
        private int _hours;
        partial void OnHoursChanged(int value) => UpdateDerivedValues();

        [ObservableProperty]
        private int _minutes;
        partial void OnMinutesChanged(int value) => UpdateDerivedValues();

        [ObservableProperty]
        private int _seconds;
        partial void OnSecondsChanged(int value) => UpdateDerivedValues();

        [ObservableProperty]
        private int _wordDuration;

        [ObservableProperty]
        private int _fixationDuration;
        partial void OnFixationDurationChanged(int value) => UpdateDerivedValues();

        [ObservableProperty]
        private int _visualCueDuration;
        partial void OnVisualCueDurationChanged(int value) => UpdateDerivedValues();

        [ObservableProperty]
        private bool _hasVisualCue;
        partial void OnHasVisualCueChanged(bool value)
        {
            if (!value)
            {
                VisualCueDuration = 0;
                DominantPercent = 50;
                SwitchPercent = null;
            }
            else
            {
                SwitchPercent = 50;
            }
            UpdateDerivedValues();
        }

        [ObservableProperty]
        private int _groupSize;

        [ObservableProperty]
        private int _taskDuration;

        [ObservableProperty]
        private int _wordCount;
        partial void OnWordCountChanged(int value) => UpdateDerivedValues();

        [ObservableProperty]
        private int _maxReactionTime;
        partial void OnMaxReactionTimeChanged(int value) => UpdateDerivedValues();

        [ObservableProperty]
        private CalculationMode _calculationMode;
        partial void OnCalculationModeChanged(CalculationMode value) => UpdateDerivedValues();

        [ObservableProperty]
        private int _dominantPercent;

        [ObservableProperty]
        private int _congruencePercent;

        [ObservableProperty]
        private int? _switchPercent;

        [ObservableProperty]
        private string _taskLanguage = "fr";

        public ProfileEditorViewModel(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles, IProfileService profileService)
        {
            _profileService = profileService;
            Profiles = profiles;
            SwitchSettingsViewModel = new SwitchSettingsViewModel();

            Profile = Profiles.Contains(profile) ? profile.CloneProfile() : profile;

            InitializeFromProfile();
        }

        private void InitializeFromProfile()
        {
            _isUpdating = true; // Stop auto-update during the initialisation

            ProfileName = Profile.ProfileName;
            Hours = Profile.Hours;
            Minutes = Profile.Minutes;
            Seconds = Profile.Seconds;
            WordDuration = Profile.WordDuration;
            FixationDuration = Profile.FixationDuration;
            VisualCueDuration = Profile.VisualCueDuration;
            HasVisualCue = Profile.HasVisualCue;
            GroupSize = Profile.GroupSize;
            TaskDuration = Profile.TaskDuration;
            WordCount = Profile.WordCount;
            MaxReactionTime = Profile.MaxReactionTime;
            CalculationMode = Profile.CalculationMode;
            DominantPercent = Profile.DominantPercent;
            CongruencePercent = Profile.CongruencePercent;
            SwitchPercent = Profile.SwitchPercent;
            TaskLanguage = Profile.TaskLanguage;

            SelectedTaskLanguage = Languages.FirstOrDefault(l => l.Code == TaskLanguage) ?? Languages[0];

            _isUpdating = false;
            UpdateDerivedValues();
        }

        private void UpdateDerivedValues()
        {
            if (_isUpdating) return;
            _isUpdating = true; // To disable auto update during init

            WordDuration = MaxReactionTime + FixationDuration + VisualCueDuration;

            if (CalculationMode == CalculationMode.TaskDuration)
            {
                TaskDuration = ((Hours * 3600) + (Minutes * 60) + Seconds) * 1000;
                if (WordDuration > 0)
                {
                    WordCount = TaskDuration / WordDuration;
                }
            }
            else if (CalculationMode == CalculationMode.WordCount)
            {
                TaskDuration = WordCount * WordDuration;
                Hours = TaskDuration / 3600000;
                Minutes = (TaskDuration % 3600000) / 60000;
                Seconds = (TaskDuration % 60000) / 1000;
            }

            SyncToProfile();
            _isUpdating = false;
        }

        private void SyncToProfile()
        {
            if (Profile == null) return;

            Profile.ProfileName = ProfileName;
            Profile.Hours = Hours;
            Profile.Minutes = Minutes;
            Profile.Seconds = Seconds;
            Profile.WordDuration = WordDuration;
            Profile.FixationDuration = FixationDuration;
            Profile.VisualCueDuration = VisualCueDuration;
            Profile.HasVisualCue = HasVisualCue;
            Profile.GroupSize = GroupSize;
            Profile.TaskDuration = TaskDuration;
            Profile.WordCount = WordCount;
            Profile.MaxReactionTime = MaxReactionTime;
            Profile.CalculationMode = CalculationMode;
            Profile.DominantPercent = DominantPercent;
            Profile.CongruencePercent = CongruencePercent;
            Profile.SwitchPercent = SwitchPercent;
            Profile.TaskLanguage = TaskLanguage;
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ProfileName))
                {
                    await ShowErrorDialogAsync(Strings.Error_ProfileNameEmpty);
                    return;
                }

                if (Profiles.Any(p => p.Id != Profile.Id && p.ProfileName == ProfileName))
                {
                    await ShowErrorDialogAsync(Strings.Error_ProfileNameExists);
                    return;
                }

                if (WordDuration > 0 && TaskDuration % WordDuration != 0)
                {
                    await ShowErrorDialogAsync(Strings.Error_TrialDurationNotDividingTaskDuration);
                    return;
                }

                int wordNumber = WordDuration > 0 ? TaskDuration / WordDuration : 0;
                if (GroupSize <= 0 || (wordNumber > 0 && wordNumber % GroupSize != 0))
                {
                    await ShowErrorDialogAsync(Strings.Error_GroupSizeInvalid);
                    return;
                }

                if (HasVisualCue && VisualCueDuration == 0)
                {
                    await ShowErrorDialogAsync(Strings.Error_VisualCueDurationInvalid);
                    return;
                }

                if (MaxReactionTime <= 0)
                {
                    await ShowErrorDialogAsync(Strings.Error_MaxResponseTimeInvalid);
                    return;
                }

                SyncToProfile();

                var updatedProfiles = _profileService.UpsertProfile(Profile);
                Profiles.Clear();
                foreach (var prof in updatedProfiles)
                {
                    Profiles.Add(prof);
                }

                var matchedProfile = Profiles.FirstOrDefault(p => p.Id == Profile.Id);
                if (matchedProfile != null)
                {
                    Profile = matchedProfile;
                    InitializeFromProfile();
                }

                DialogResult = true;
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }

        [RelayCommand]
        public void Cancel()
        {
            DialogResult = false;
            CloseAction?.Invoke();
        }
    }
}