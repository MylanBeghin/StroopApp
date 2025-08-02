using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.Services;

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
		public SwitchSettingsViewModel SwitchSettingsViewModel
		{
			get;
		}

		private readonly IProfileService _IprofileService;
		public List<LanguageOption> Languages { get; } = new()
{
	new LanguageOption { Code = "fr", DisplayName = "Français" },
	new LanguageOption { Code = "en", DisplayName = "English" }
};

		public LanguageOption SelectedInstructionsLanguage
		{
			get => Languages.FirstOrDefault(l => l.Code == Profile.InstructionsLanguage) ?? Languages[0];
			set
			{
				if (Profile.InstructionsLanguage != value?.Code)
				{
					Profile.InstructionsLanguage = value?.Code ?? "fr";
					OnPropertyChanged();
				}
			}
		}


		public ProfileEditorViewModel(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles, IProfileService profileService)
		{
			_IprofileService = profileService;
			Profiles = profiles;

			SwitchSettingsViewModel = new SwitchSettingsViewModel();

			Profile = Profiles.Contains(profile) ? profile.CloneProfile() : profile;
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



		public void Save()
		{
			if (string.IsNullOrWhiteSpace(Profile.ProfileName))
			{
				ShowErrorDialog(Strings.Error_ProfileNameEmpty);
				return;
			}

			if (Profiles.Any(p => p.Id != Profile.Id && p.ProfileName == Profile.ProfileName))
			{
				ShowErrorDialog(Strings.Error_ProfileNameExists);
				return;
			}

			if (Profile.TaskDuration % Profile.WordDuration != 0)
			{
				ShowErrorDialog(Strings.Error_TrialDurationNotDividingTaskDuration);
				return;
			}

			int wordNumber = Profile.TaskDuration / Profile.WordDuration;
			if (Profile.GroupSize <= 0 || wordNumber % Profile.GroupSize != 0)
			{
				ShowErrorDialog(Strings.Error_GroupSizeInvalid);
				return;
			}

			if (Profile.AmorceDuration == 0 && Profile.IsAmorce)
			{
				ShowErrorDialog(Strings.Error_AmorceDurationInvalid);
				return;
			}

			if (Profile.MaxReactionTime <= 0)
			{
				ShowErrorDialog(Strings.Error_MaxResponseTimeInvalid);
				return;
			}
			var updatedProfiles = _IprofileService.UpsertProfile(Profile);
			Profiles.Clear();
			foreach (var prof in updatedProfiles)
			{
				Profiles.Add(prof);
			}
			var matchedProfile = Profiles.FirstOrDefault(p => p.Id == Profile.Id);
			if (matchedProfile != null)
			{
				Profile = matchedProfile;
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
