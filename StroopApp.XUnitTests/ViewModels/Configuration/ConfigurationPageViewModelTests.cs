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
			var settings = new ExperimentSettings();
			var dummyProfileService = new DummyProfileService();
			var dummyParticipantService = new DummyParticipantService();
			var dummyKeyMappingService = new DummyKeyMappingService();
			var dummyWindowManager = new DummyWindowManager();
			var dummyNavigationService = new DummyNavigationService();
			var dummyTrialGenerationService = new DummyTrialGenerationService();

			var profileViewModel = new ProfileManagementViewModel(dummyProfileService);
			var participantViewModel = new ParticipantManagementViewModel(dummyParticipantService, false);
			var keyMappingViewModel = new KeyMappingViewModel(dummyKeyMappingService);

			var viewModel = new ConfigurationPageViewModel(
				settings,
				profileViewModel,
				participantViewModel,
				keyMappingViewModel,
				dummyNavigationService,
				dummyWindowManager,
				dummyTrialGenerationService);

			var dummyProfile = new ExperimentProfile
			{
				ProfileName = "TestProfile",
				WordCount = 10
			};
			profileViewModel.CurrentProfile = dummyProfile;

			var dummyParticipant = new Participant { Id = "15j16" };
			participantViewModel.SelectedParticipant = dummyParticipant;

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);

			// Assert
			Assert.True(dummyNavigationService.Navigated);
			Assert.True(dummyWindowManager.ShowCalled);
			Assert.True(dummyTrialGenerationService.GenerateTrialsCalled);
			Assert.Equal(dummyProfile, settings.CurrentProfile);
			Assert.Equal(dummyParticipant, settings.Participant);
			Assert.NotNull(settings.ExperimentContext.CurrentBlock);
			Assert.Equal(10, dummyTrialGenerationService.LastTrialCount);
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
			var dummyTrialGenerationService = new DummyTrialGenerationService();

			var profileViewModel = new ProfileManagementViewModel(dummyProfileService);
			// No CurrentProfile set intentionally

			var participantViewModel = new ParticipantManagementViewModel(dummyParticipantService, false);
			participantViewModel.SelectedParticipant = new Participant { Id = "15h61" };

			var keyMappingViewModel = new KeyMappingViewModel(dummyKeyMappingService);

			var viewModel = new TestableConfigurationPageViewModel(
				settings,
				profileViewModel,
				participantViewModel,
				keyMappingViewModel,
				dummyNavigationService,
				dummyWindowManager,
				dummyTrialGenerationService);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);

			// Assert
			Assert.True(viewModel.ErrorDialogShown);
			Assert.Equal(Strings.Error_SelectProfile, viewModel.LastErrorMessage);
			Assert.False(dummyNavigationService.Navigated);
			Assert.False(dummyWindowManager.ShowCalled);
			Assert.False(dummyTrialGenerationService.GenerateTrialsCalled);
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
			var dummyTrialGenerationService = new DummyTrialGenerationService();

			var profileViewModel = new ProfileManagementViewModel(dummyProfileService);
			var dummyProfile = new ExperimentProfile { ProfileName = "TestProfile" };
			profileViewModel.CurrentProfile = dummyProfile;

			var participantViewModel = new ParticipantManagementViewModel(dummyParticipantService, false);
			// No SelectedParticipant set intentionally

			var keyMappingViewModel = new KeyMappingViewModel(dummyKeyMappingService);

			var viewModel = new TestableConfigurationPageViewModel(
				settings,
				profileViewModel,
				participantViewModel,
				keyMappingViewModel,
				dummyNavigationService,
				dummyWindowManager,
				dummyTrialGenerationService);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);

			// Assert
			Assert.True(viewModel.ErrorDialogShown);
			Assert.Equal(Strings.Error_SelectParticipant, viewModel.LastErrorMessage);
			Assert.False(dummyNavigationService.Navigated);
			Assert.False(dummyWindowManager.ShowCalled);
			Assert.False(dummyTrialGenerationService.GenerateTrialsCalled);
		}

		[Fact]
		public void LaunchExperiment_WithValidSettings_GeneratesTrialsCorrectly()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var dummyTrialGenerationService = new DummyTrialGenerationService();
			var dummyWindowManager = new DummyWindowManager();
			var dummyNavigationService = new DummyNavigationService();

			var profileViewModel = new ProfileManagementViewModel(new DummyProfileService());
			var participantViewModel = new ParticipantManagementViewModel(new DummyParticipantService(), false);
			var keyMappingViewModel = new KeyMappingViewModel(new DummyKeyMappingService());

			var viewModel = new ConfigurationPageViewModel(
				settings,
				profileViewModel,
				participantViewModel,
				keyMappingViewModel,
				dummyNavigationService,
				dummyWindowManager,
				dummyTrialGenerationService);

			var profile = new ExperimentProfile
			{
				ProfileName = "TestProfile",
				WordCount = 20
			};
			profileViewModel.CurrentProfile = profile;
			participantViewModel.SelectedParticipant = new Participant { Id = "TestParticipant" };

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);

			// Assert
			Assert.True(dummyTrialGenerationService.GenerateTrialsCalled);
			Assert.Equal(20, dummyTrialGenerationService.LastTrialCount);
			Assert.NotNull(settings.ExperimentContext.CurrentBlock);
			Assert.Equal(20, settings.ExperimentContext.CurrentBlock.TrialRecords.Count);
		}
	}
}
