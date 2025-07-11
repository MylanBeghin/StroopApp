﻿using System.Collections.ObjectModel;
using System.Windows.Input;

using ModernWpf.Controls;

using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services;
using StroopApp.Views;

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
				var updatedProfiles = _profileService.UpsertProfile(newProfile);
				Profiles.Clear();
				foreach (var prof in updatedProfiles)
				{
					Profiles.Add(prof);
				}
				CurrentProfile = Profiles.FirstOrDefault(p => p.Id == newProfile.Id);
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
				_profileService.UpsertProfile(
					viewModel.ModifiedProfile);
				CurrentProfile = viewModel.ModifiedProfile;
				OnPropertyChanged(nameof(CurrentProfile));
			}
		}
		private async void DeleteProfile()
		{
			if (CurrentProfile == null)
			{
				ShowErrorDialog("Veuillez sélectionner un profil à supprimer !");
				return;
			}
			var dlg = new ContentDialog
			{
				Title = "Confirmation de suppression",
				Content = "Voulez-vous vraiment supprimer ce profil ?",
				PrimaryButtonText = "Supprimer",
				CloseButtonText = "Annuler"
			};
			if (await dlg.ShowAsync() != ContentDialogResult.Primary)
				return;
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
