using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.Services.Profile;

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
		public ObservableCollection<ExperimentProfile> Profiles { get; }
		public bool? DialogResult { get; private set; }
		public Action? CloseAction { get; set; }
		public ICommand SaveCommand { get; }
		public ICommand CancelCommand { get; }
		public SwitchSettingsViewModel SwitchSettingsViewModel { get; }

		private readonly IProfileService _IprofileService;
		
		public List<LanguageOption> Languages { get; } = new()
		{
			new LanguageOption { Code = "fr", DisplayName = "Français" },
			new LanguageOption { Code = "en", DisplayName = "English" }
		};

		public LanguageOption SelectedTaskLanguage
		{
			get => Languages.FirstOrDefault(l => l.Code == Profile.TaskLanguage) ?? Languages[0];
			set
			{
				if (Profile.TaskLanguage != value?.Code)
				{
					Profile.TaskLanguage = value?.Code ?? "fr";
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

			SaveCommand = new RelayCommand(async _ => await SaveAsync());
			CancelCommand = new RelayCommand(_ => Cancel());

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

		public async Task SaveAsync()
		{
			try
			{
				if (string.IsNullOrWhiteSpace(Profile.ProfileName))
				{
					await ShowErrorDialogAsync(Strings.Error_ProfileNameEmpty);
					return;
				}

				if (Profiles.Any(p => p.Id != Profile.Id && p.ProfileName == Profile.ProfileName))
				{
					await ShowErrorDialogAsync(Strings.Error_ProfileNameExists);
					return;
				}

				if (Profile.TaskDuration % Profile.WordDuration != 0)
				{
					await ShowErrorDialogAsync(Strings.Error_TrialDurationNotDividingTaskDuration);
					return;
				}

				int wordNumber = Profile.TaskDuration / Profile.WordDuration;
				if (Profile.GroupSize <= 0 || wordNumber % Profile.GroupSize != 0)
				{
					await ShowErrorDialogAsync(Strings.Error_GroupSizeInvalid);
					return;
				}

				if (Profile.AmorceDuration == 0 && Profile.IsAmorce)
				{
					await ShowErrorDialogAsync(Strings.Error_AmorceDurationInvalid);
					return;
				}

				if (Profile.MaxReactionTime <= 0)
				{
					await ShowErrorDialogAsync(Strings.Error_MaxResponseTimeInvalid);
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
			catch (Exception ex)
			{
				await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
			}
		}

		public void Cancel()
		{
			DialogResult = false;
			CloseAction?.Invoke();
		}
	}
}
