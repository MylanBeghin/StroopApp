using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.Services.Navigation;
using StroopApp.Services.Trial;
using StroopApp.Services.Window;
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
		public async Task LaunchExperiment_WithProfileAndParticipant_NavigatesAndOpensWindow()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var dummyProfileService = new DummyProfileService();
			var dummyParticipantService = new DummyParticipantService();
			var dummyKeyMappingService = new DummyKeyMappingService();
			var dummyWindowManager = new DummyWindowManager();
			var dummyNavigationService = new DummyNavigationService();
			var dummyTrialGenerationService = new DummyTrialGenerationService();
			var dummyLanguageService = new DummyLanguageService();


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
				dummyTrialGenerationService,
				dummyLanguageService);

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
			await Task.Delay(100); // Attendre la fin de l'exécution async

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
		public async Task LaunchExperiment_WithoutProfile_ShowsErrorDialog()
		{
			// Arrange
			var settings = new ExperimentSettings();
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

			var viewModel = new TestableConfigurationPageViewModel(
				settings,
				profileViewModel,
				participantViewModel,
				keyMappingViewModel,
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
			var settings = new ExperimentSettings();
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

			var viewModel = new TestableConfigurationPageViewModel(
				settings,
				profileViewModel,
				participantViewModel,
				keyMappingViewModel,
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

		[Fact]
		public async Task LaunchExperiment_WithValidSettings_GeneratesTrialsCorrectly()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var dummyTrialGenerationService = new DummyTrialGenerationService();
			var dummyWindowManager = new DummyWindowManager();
			var dummyNavigationService = new DummyNavigationService();
			var dummyLanguageService = new DummyLanguageService();

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
				dummyTrialGenerationService, 
				dummyLanguageService);

			var profile = new ExperimentProfile
			{
				ProfileName = "TestProfile",
				WordCount = 20
			};
			profileViewModel.CurrentProfile = profile;
			participantViewModel.SelectedParticipant = new Participant { Id = "TestParticipant" };

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);
			await Task.Delay(100); // Attendre la fin de l'exécution async

			// Assert
			Assert.True(dummyTrialGenerationService.GenerateTrialsCalled);
			Assert.Equal(20, dummyTrialGenerationService.LastTrialCount);
			Assert.NotNull(settings.ExperimentContext.CurrentBlock);
			Assert.Equal(20, settings.ExperimentContext.CurrentBlock.TrialRecords.Count);
		}

		// ========== CHARACTERIZATION TESTS FOR LaunchExperiment WORKFLOW ==========

		[Fact]
		public async Task LaunchExperiment_SetsExperimentContextIsTaskStoppedToFalse()
		{
			// Arrange
			var settings = new ExperimentSettings();
			settings.ExperimentContext.IsTaskStopped = true; // Pre-set to true
			var viewModel = CreateConfiguredViewModel(settings, out var dummies);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);
			await Task.Delay(100);

			// Assert
			Assert.False(settings.ExperimentContext.IsTaskStopped);
		}

		[Fact]
		public async Task LaunchExperiment_SetsExperimentContextIsBlockFinishedToFalse()
		{
			// Arrange
			var settings = new ExperimentSettings();
			settings.ExperimentContext.IsBlockFinished = true; // Pre-set to true
			var viewModel = CreateConfiguredViewModel(settings, out var dummies);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);
			await Task.Delay(100);

			// Assert
			Assert.False(settings.ExperimentContext.IsBlockFinished);
		}

		[Fact]
		public async Task LaunchExperiment_CreatesNewColumnSerie()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var originalColumnSerie = settings.ExperimentContext.ColumnSerie;
			var viewModel = CreateConfiguredViewModel(settings, out var dummies);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);
			await Task.Delay(100);

			// Assert
			Assert.NotNull(settings.ExperimentContext.ColumnSerie);
			Assert.NotSame(originalColumnSerie, settings.ExperimentContext.ColumnSerie);
		}

		[Fact]
		public async Task LaunchExperiment_AddsBlockToExperimentContext()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var initialBlockCount = settings.ExperimentContext.Blocks.Count;
			var viewModel = CreateConfiguredViewModel(settings, out var dummies);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);
			await Task.Delay(100);

			// Assert
			Assert.Equal(initialBlockCount + 1, settings.ExperimentContext.Blocks.Count);
		}

		[Fact]
		public async Task LaunchExperiment_SetsCurrentBlockInExperimentContext()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var viewModel = CreateConfiguredViewModel(settings, out var dummies);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);
			await Task.Delay(100);

			// Assert
			Assert.NotNull(settings.ExperimentContext.CurrentBlock);
			Assert.Equal(settings.Block, settings.ExperimentContext.CurrentBlock.BlockNumber);
		}

		[Fact]
		public async Task LaunchExperiment_PopulatesTrialRecordsInCurrentBlock()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var viewModel = CreateConfiguredViewModel(settings, out var dummies);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);
			await Task.Delay(100);

			// Assert
			Assert.NotEmpty(settings.ExperimentContext.CurrentBlock.TrialRecords);
			Assert.Equal(10, settings.ExperimentContext.CurrentBlock.TrialRecords.Count);
		}

		[Fact]
		public async Task LaunchExperiment_CopiesKeyMappingsFromViewModel()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var viewModel = CreateConfiguredViewModel(settings, out var dummies);
			var keyMappings = dummies.keyMappingViewModel.Mappings;

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);
			await Task.Delay(100);

			// Assert
			Assert.Same(keyMappings, settings.KeyMappings);
		}

		[Fact]
		public async Task LaunchExperiment_ExecutionOrder_SetsSettingsBeforeGeneratingTrials()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var trialGenService = new OrderTrackingTrialGenerationService();
			var viewModel = CreateConfiguredViewModel(settings, trialGenService, out var dummies);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);
			await Task.Delay(100);

			// Assert - Verify settings were set before GenerateTrials was called
			Assert.True(trialGenService.GenerateTrialsCalled);
			Assert.NotNull(trialGenService.CapturedSettingsOnGenerate);
			Assert.Same(dummies.profile, trialGenService.CapturedSettingsOnGenerate.CurrentProfile);
			Assert.Same(dummies.participant, trialGenService.CapturedSettingsOnGenerate.Participant);
		}

		[Fact]
		public async Task LaunchExperiment_ExecutionOrder_AddsSerieBeforePopulatingTrials()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var viewModel = CreateConfiguredViewModel(settings, out var dummies);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);
			await Task.Delay(100);

			// Assert - CurrentBlock should be set before trials are added
			Assert.NotNull(settings.ExperimentContext.CurrentBlock);
			Assert.Equal(10, settings.ExperimentContext.CurrentBlock.TrialRecords.Count);
		}

		[Fact]
		public async Task LaunchExperiment_CallsNavigationServiceExactlyOnce()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var viewModel = CreateConfiguredViewModel(settings, out var dummies);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);
			await Task.Delay(100);

			// Assert
			Assert.Equal(1, dummies.navigationService.NavigationCount);
		}

		[Fact]
		public async Task LaunchExperiment_CallsWindowManagerShowExactlyOnce()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var viewModel = CreateConfiguredViewModel(settings, out var dummies);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);
			await Task.Delay(100);

			// Assert
			Assert.Equal(1, dummies.windowManager.ShowCallCount);
		}

		[Fact]
		public async Task LaunchExperiment_PassesCorrectSettingsToWindowManager()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var viewModel = CreateConfiguredViewModel(settings, out var dummies);

			// Act
			viewModel.LaunchExperimentCommand.Execute(null);
			await Task.Delay(100);

			// Assert
			Assert.Same(settings, dummies.windowManager.LastPassedSettings);
		}

		// ========== HELPER METHODS ==========

		private ConfigurationPageViewModel CreateConfiguredViewModel(
			ExperimentSettings settings,
			out (ExperimentProfile profile, Participant participant, CountingNavigationService navigationService, 
			CountingWindowManager windowManager, KeyMappingViewModel keyMappingViewModel) dummies)
		{
			var profile = new ExperimentProfile
			{
				ProfileName = "TestProfile",
				WordCount = 10
			};
			var participant = new Participant { Id = "P001" };

			var profileViewModel = new ProfileManagementViewModel(new DummyProfileService());
			profileViewModel.CurrentProfile = profile;

			var participantViewModel = new ParticipantManagementViewModel(new DummyParticipantService(), false);
			participantViewModel.SelectedParticipant = participant;

			var keyMappingViewModel = new KeyMappingViewModel(new DummyKeyMappingService());

			var navigationService = new CountingNavigationService();
			var windowManager = new CountingWindowManager();
			var trialGenService = new DummyTrialGenerationService();
			var languageService = new DummyLanguageService();

			dummies = (profile, participant, navigationService, windowManager, keyMappingViewModel);

			return new ConfigurationPageViewModel(
				settings,
				profileViewModel,
				participantViewModel,
				keyMappingViewModel,
				navigationService,
				windowManager,
				trialGenService,
				languageService);
		}

		private ConfigurationPageViewModel CreateConfiguredViewModel(
			ExperimentSettings settings,
			ITrialGenerationService trialGenService,
			out (ExperimentProfile profile, Participant participant, CountingNavigationService navigationService, 
			CountingWindowManager windowManager, KeyMappingViewModel keyMappingViewModel) dummies)
		{
			var profile = new ExperimentProfile
			{
				ProfileName = "TestProfile",
				WordCount = 10
			};
			var participant = new Participant { Id = "P001" };

			var profileViewModel = new ProfileManagementViewModel(new DummyProfileService());
			profileViewModel.CurrentProfile = profile;

			var participantViewModel = new ParticipantManagementViewModel(new DummyParticipantService(), false);
			participantViewModel.SelectedParticipant = participant;

			var keyMappingViewModel = new KeyMappingViewModel(new DummyKeyMappingService());

			var navigationService = new CountingNavigationService();
			var windowManager = new CountingWindowManager();
			var languageService = new DummyLanguageService();

			dummies = (profile, participant, navigationService, windowManager, keyMappingViewModel);

			return new ConfigurationPageViewModel(
				settings,
				profileViewModel,
				participantViewModel,
				keyMappingViewModel,
				navigationService,
				windowManager,
				trialGenService,
				languageService);
		}

		// ========== TEST HELPERS ==========

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
		}

		private class CountingWindowManager : IWindowManager
		{
			public int ShowCallCount { get; private set; }
			public ExperimentSettings LastPassedSettings { get; private set; }

			public void ShowParticipantWindow(ExperimentSettings settings)
			{
				ShowCallCount++;
				LastPassedSettings = settings;
			}

			public void CloseParticipantWindow()
			{
			}
		}

		private class OrderTrackingTrialGenerationService : ITrialGenerationService
		{
			public bool GenerateTrialsCalled { get; private set; }
			public ExperimentSettings CapturedSettingsOnGenerate { get; private set; }

			public List<StroopTrial> GenerateTrials(ExperimentSettings settings)
			{
				GenerateTrialsCalled = true;
				CapturedSettingsOnGenerate = settings;

				var trials = new List<StroopTrial>();
				for (int i = 0; i < settings.CurrentProfile.WordCount; i++)
				{
					trials.Add(new StroopTrial
					{
						TrialNumber = i + 1,
						Block = settings.Block,
						ParticipantId = settings.Participant.Id
					});
				}
				return trials;
			}

			public List<AmorceType> GenerateAmorceSequence(int count, int switchPercentage)
			{
				return new List<AmorceType>();
			}
		}
	}
}
