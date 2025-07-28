using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.ViewModels.Configuration;
using StroopApp.ViewModels.Configuration.Participant;
using StroopApp.ViewModels.Configuration.Profile;
using StroopApp.XUnitTests.TestDummies;

using Xunit;

namespace StroopApp.XUnitTests.ViewModels.Configuration
{
	public class ConfigurationPageViewModelTests
	{
		[Fact]
		public void LaunchExperiment_WithProfileAndParticipant_NavigatesAndOpensWindow()
		{
			// Arrange
			ExperimentSettings? settings = new ExperimentSettings();
			// Services
			DummyProfileService? dummyProfileService = new DummyProfileService();
			DummyParticipantService? dummyParticipantService = new DummyParticipantService();
			DummyKeyMappingService? dummyKeyMappingService = new DummyKeyMappingService();
			DummyExportationService? dummyExportationService = new DummyExportationService();
			DummyWindowManager? dummyWindowManager = new DummyWindowManager();
			DummyNavigationService? dummyNavigationService = new DummyNavigationService();

			//ViewModels
			ProfileManagementViewModel? profileViewModel = new ProfileManagementViewModel(profileService: dummyProfileService);
			ParticipantManagementViewModel? participantViewModel = new ParticipantManagementViewModel(participantService: dummyParticipantService, isParticipantSelectionEnabled: false); // We assume it's the first experiment launched.
			KeyMappingViewModel? keyMappingViewModel = new KeyMappingViewModel(keyMappingService: dummyKeyMappingService);

			var viewModel = new ConfigurationPageViewModel(
			settings,
			profileViewModel,
			participantViewModel,
			keyMappingViewModel,
			dummyNavigationService,
			dummyWindowManager
			);
			// Current Participant Selected
			ExperimentProfile? dummyProfile = new ExperimentProfile { ProfileName = "TestProfile" };
			profileViewModel.CurrentProfile = dummyProfile;

			Participant? dummyParticipant = new Participant { Id = "15j16" };
			participantViewModel.SelectedParticipant = dummyParticipant;

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);

			// Arrange
			Assert.True(dummyNavigationService.Navigated);
			Assert.True(dummyWindowManager.ShowCalled);
			Assert.Equal(dummyProfile, settings.CurrentProfile);
			Assert.Equal(dummyParticipant, settings.Participant);
		}

		[Fact]
		public void LaunchExperiment_WithoutProfile_ShowsErrorDialog()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var dummyProfileService = new DummyProfileService();
			var dummyParticipantService = new DummyParticipantService();
			var dummyKeyMappingService = new DummyKeyMappingService();
			var dummyWindowManager = new DummyWindowManager();
			var dummyNavigationService = new DummyNavigationService();

			var profileViewModel = new ProfileManagementViewModel(dummyProfileService);
			// No CurrentProfile !

			var participantViewModel = new ParticipantManagementViewModel(dummyParticipantService, false);
			participantViewModel.SelectedParticipant = new Participant { Id = "15h61" };

			var keyMappingViewModel = new KeyMappingViewModel(dummyKeyMappingService);

			var viewModel = new TestableConfigurationPageViewModel(
				settings,
				profileViewModel,
				participantViewModel,
				keyMappingViewModel,
				dummyNavigationService,
				dummyWindowManager
			);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);

			// Assert
			Assert.True(viewModel.ErrorDialogShown);
			Assert.Equal(Strings.Error_SelectProfile, viewModel.LastErrorMessage);
			Assert.False(dummyNavigationService.Navigated);
			Assert.False(dummyWindowManager.ShowCalled);
		}
		[Fact]
		public void LaunchExperiment_WithoutParticipant_ShowsErrorDialog()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var dummyProfileService = new DummyProfileService();
			var dummyParticipantService = new DummyParticipantService();
			var dummyKeyMappingService = new DummyKeyMappingService();
			var dummyWindowManager = new DummyWindowManager();
			var dummyNavigationService = new DummyNavigationService();

			var profileViewModel = new ProfileManagementViewModel(dummyProfileService);
			ExperimentProfile? dummyProfile = new ExperimentProfile { ProfileName = "TestProfile" };
			profileViewModel.CurrentProfile = dummyProfile;

			var participantViewModel = new ParticipantManagementViewModel(dummyParticipantService, false);
			// No SelectedParticipant !

			var keyMappingViewModel = new KeyMappingViewModel(dummyKeyMappingService);

			var viewModel = new TestableConfigurationPageViewModel(
				settings,
				profileViewModel,
				participantViewModel,
				keyMappingViewModel,
				dummyNavigationService,
				dummyWindowManager
			);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);

			// Assert
			Assert.True(viewModel.ErrorDialogShown);
			Assert.Equal(Strings.Error_SelectParticipant, viewModel.LastErrorMessage);
			Assert.False(dummyNavigationService.Navigated);
			Assert.False(dummyWindowManager.ShowCalled);
		}
	}
}
