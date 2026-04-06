using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.Services.Navigation;
using StroopApp.ViewModels.Configuration;
using StroopApp.ViewModels.Configuration.Participant;
using StroopApp.ViewModels.Configuration.Profile;
using StroopApp.ViewModels.State;
using StroopApp.XUnitTests.TestDummies;

using Xunit;

namespace StroopApp.XUnitTests.ViewModels.Configuration
{
	public class ConfigurationPageViewModelTests
	{
		[Fact]
		public async Task LaunchExperiment_WithoutProfile_ShowsErrorDialog()
		{
			// Arrange
			var settingsModel = new ExperimentSettings();
			var settings = new ExperimentSettingsViewModel(settingsModel);
			var dummyProfileService = new DummyProfileService();
			var dummyParticipantService = new DummyParticipantService();
			var dummyKeyMappingService = new DummyKeyMappingService();
			var dummyWindowManager = new DummyWindowManager();
			var dummyNavigationService = new DummyNavigationService();
			var dummyTrialGenerationService = new DummyTrialGenerationService();
			var dummyLanguageService = new DummyLanguageService();

            var profileViewModel = new ProfileManagementViewModel(dummyProfileService);
			// No CurrentProfile set intentionally

			var participantViewModel = new ParticipantManagementViewModel(dummyParticipantService, false);
			participantViewModel.SelectedParticipant = new Participant { Id = "15h61" };

			var keyMappingViewModel = new KeyMappingViewModel(dummyKeyMappingService);

			var dummyExportationService = new DummyExportationService();
			var exportFolderSelectorViewModel = new ExportFolderSelectorViewModel(settings, dummyExportationService);

			var viewModel = new TestableConfigurationPageViewModel(
				settings,
				profileViewModel,
				participantViewModel,
				keyMappingViewModel,
				exportFolderSelectorViewModel,
				dummyNavigationService,
				dummyWindowManager,
				dummyTrialGenerationService, 
				dummyLanguageService);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);
			await Task.Delay(100); // Attendre la fin de l'exécution async

			// Assert
			Assert.True(viewModel.ErrorDialogShown);
			Assert.Equal(Strings.Error_SelectProfile, viewModel.LastErrorMessage);
			Assert.False(dummyNavigationService.Navigated);
			Assert.False(dummyWindowManager.ShowCalled);
			Assert.False(dummyTrialGenerationService.GenerateTrialsCalled);
		}

		[Fact]
		public async Task LaunchExperiment_WithoutParticipant_ShowsErrorDialog()
		{
            // Arrange
            var settingsModel = new ExperimentSettings();
            var settings = new ExperimentSettingsViewModel(settingsModel);
            var dummyProfileService = new DummyProfileService();
			var dummyParticipantService = new DummyParticipantService();
			var dummyKeyMappingService = new DummyKeyMappingService();
			var dummyWindowManager = new DummyWindowManager();
			var dummyNavigationService = new DummyNavigationService();
			var dummyTrialGenerationService = new DummyTrialGenerationService();
			var dummyLanguageService = new DummyLanguageService();

            var profileViewModel = new ProfileManagementViewModel(dummyProfileService);
			var dummyProfile = new ExperimentProfile { ProfileName = "TestProfile" };
			profileViewModel.CurrentProfile = dummyProfile;

			var participantViewModel = new ParticipantManagementViewModel(dummyParticipantService, false);

			var keyMappingViewModel = new KeyMappingViewModel(dummyKeyMappingService);

			var dummyExportationService = new DummyExportationService();
			var exportFolderSelectorViewModel = new ExportFolderSelectorViewModel(settings, dummyExportationService);

			var viewModel = new TestableConfigurationPageViewModel(
				settings,
				profileViewModel,
				participantViewModel,
				keyMappingViewModel,
				exportFolderSelectorViewModel,
				dummyNavigationService,
				dummyWindowManager,
				dummyTrialGenerationService, 
				dummyLanguageService);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);
			await Task.Delay(100); // Attendre la fin de l'exécution async

			// Assert
			Assert.True(viewModel.ErrorDialogShown);
			Assert.Equal(Strings.Error_SelectParticipant, viewModel.LastErrorMessage);
			Assert.False(dummyNavigationService.Navigated);
			Assert.False(dummyWindowManager.ShowCalled);
			Assert.False(dummyTrialGenerationService.GenerateTrialsCalled);
		}

		private class CountingNavigationService : INavigationService
		{
			public int NavigationCount { get; private set; }

			public void NavigateTo(Func<System.Windows.Controls.Page> pageFactory)
			{
				NavigationCount++;
			}

			public void NavigateTo<T>(object parameter = null) where T : System.Windows.Controls.Page
			{
				NavigationCount++;
			}

			public void SetFrame(System.Windows.Controls.Frame frame)
			{
			}
		}

	}
}
