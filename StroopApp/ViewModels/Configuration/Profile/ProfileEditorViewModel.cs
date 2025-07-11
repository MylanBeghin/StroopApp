﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

using StroopApp.Core;
using StroopApp.Models;
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
				ShowErrorDialog("Le nom du profil ne peut pas être vide ou contenir uniquement des espaces.");
				return;
			}

			if (Profiles.Any(p => p.Id != Profile.Id && p.ProfileName == Profile.ProfileName))
			{
				ShowErrorDialog("Un profil avec ce nom existe déjà. Veuillez choisir un autre nom.");
				return;
			}

			if (Profile.WordDuration <= 0 || Profile.TaskDuration % Profile.WordDuration != 0)
			{
				ShowErrorDialog("La durée du mot doit être positive et TaskDuration doit être divisible par WordDuration.");
				return;
			}

			int wordNumber = Profile.TaskDuration / Profile.WordDuration;
			if (Profile.GroupSize <= 0 || wordNumber % Profile.GroupSize != 0)
			{
				ShowErrorDialog("La taille du groupe doit être positive et diviser le nombre de mots.");
				return;
			}

			if (Profile.AmorceDuration == 0 && Profile.IsAmorce)
			{
				ShowErrorDialog("Le temps d'amorce est doit être supérieur à 0 !");
				return;
			}

			if (Profile.MaxReactionTime <= 0)
			{
				ShowErrorDialog("Le temps de réaction maximum doit être positif.");
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
