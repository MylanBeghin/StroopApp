using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Services.Trial;
using StroopApp.Services.Window;
using StroopApp.Services.Language;
using StroopApp.ViewModels.Configuration;
using StroopApp.ViewModels.Configuration.Participant;
using StroopApp.ViewModels.Configuration.Profile;

namespace StroopApp.XUnitTests.ViewModels.Configuration
{
	public class TestableConfigurationPageViewModel : ConfigurationPageViewModel
	{
		public bool ErrorDialogShown = false;
		public string? LastErrorMessage = null;
		
		public TestableConfigurationPageViewModel(
			ExperimentSettings settings,
			ProfileManagementViewModel profileViewModel,
			ParticipantManagementViewModel participantViewModel,
			KeyMappingViewModel keyMappingViewModel,
			INavigationService experimenterNavigationService,
			IWindowManager windowManager,
			ITrialGenerationService trialGenerationService,
			ILanguageService languageService
		) : base(settings, profileViewModel, participantViewModel, keyMappingViewModel, experimenterNavigationService, windowManager, trialGenerationService, languageService)
		{ }

		protected override Task ShowErrorDialogAsync(string message)
		{
			ErrorDialogShown = true;
			LastErrorMessage = message;
			return Task.CompletedTask;
		}
	}
}
