﻿using System.Windows;
using System.Windows.Input;


using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.Services.Language;
using StroopApp.Services.Navigation;
using StroopApp.Services.Trial;
using StroopApp.Services.Window;
using StroopApp.ViewModels.Configuration.Participant;
using StroopApp.ViewModels.Configuration.Profile;
using StroopApp.Views;
using StroopApp.Views.Configuration;

namespace StroopApp.ViewModels.Configuration
{
	public class ConfigurationPageViewModel : ViewModelBase
	{
		private readonly ProfileManagementViewModel _profileViewModel;
		private readonly ParticipantManagementViewModel _participantViewModel;
		private readonly KeyMappingViewModel _keyMappingViewModel;
		private readonly INavigationService _experimenterNavigationService;
		private readonly IWindowManager _windowManager;
		private readonly ILanguageService _languageService;
		private readonly ITrialGenerationService _trialGenerationService;
		public ExperimentSettings _settings
		{
			get; set;
		}
		public ICommand LaunchExperimentCommand
		{
			get;
		}
		public ICommand OpenAdvancedSettingsCommand
		{
			get;
		}
		public ConfigurationPageViewModel(ExperimentSettings settings,
								  ProfileManagementViewModel profileViewModel,
								  ParticipantManagementViewModel participantViewModel,
								  KeyMappingViewModel keyMappingViewModel,
								  INavigationService experimenterNavigationService,
								  IWindowManager windowManager, ITrialGenerationService trialGenerationService
								  )
		{
			_profileViewModel = profileViewModel;
			_participantViewModel = participantViewModel;
			_keyMappingViewModel = keyMappingViewModel;
			_experimenterNavigationService = experimenterNavigationService;
			_windowManager = windowManager;
			_trialGenerationService = trialGenerationService;
			_settings = settings;
			LaunchExperimentCommand = new RelayCommand(LaunchExperiment);
			OpenAdvancedSettingsCommand = new RelayCommand(OpenAdvancedSettings);
		}


		private void LaunchExperiment()
		{
			_settings.CurrentProfile = _profileViewModel.CurrentProfile;
			_settings.Participant = _participantViewModel.SelectedParticipant;
			_settings.KeyMappings = _keyMappingViewModel.Mappings;

			if (_settings.CurrentProfile == null)
			{
				ShowErrorDialog(Strings.Error_SelectProfile);
				return;
			}

			if (_settings.Participant == null)
			{
				ShowErrorDialog(Strings.Error_SelectParticipant);
				return;
			}
			_settings.ExperimentContext.IsTaskStopped = false;
			_settings.ExperimentContext.IsBlockFinished = false;
			_settings.ExperimentContext.NewColumnSerie();
			_settings.ExperimentContext.AddNewSerie(_settings);
			try
			{
				var trials = _trialGenerationService.GenerateTrials(_settings);

				foreach (var trial in trials)
				{
					_settings.ExperimentContext.CurrentBlock.TrialRecords.Add(trial);
				}
				_experimenterNavigationService.NavigateTo(() =>
					new ExperimentDashBoardPage(_settings, _experimenterNavigationService, _windowManager));
				_windowManager.ShowParticipantWindow(_settings);
			}
			catch (Exception ex)
			{
				ShowErrorDialog($"Error while generating trials : {ex.Message}");
			}
		}
		private void OpenAdvancedSettings()
		{
			var window = new AdvancedSettingsWindow();
			window.Owner = Application.Current.MainWindow;
			window.ShowDialog();
		}
	}
}
