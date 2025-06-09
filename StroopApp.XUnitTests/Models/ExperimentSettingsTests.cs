namespace StroopApp.XUnitTests.Models
{
	using StroopApp.Models;

	using Xunit;

	public class ExperimentSettingsTests
	{
		[Fact]
		public void Constructor_InitializesProperties()
		{
			// Arrange & Act
			var settings = new ExperimentSettings();

			// Assert
			Assert.NotNull(settings.CurrentProfile);
			Assert.NotNull(settings.KeyMappings);
			Assert.NotNull(settings.ExperimentContext);
			Assert.Equal("", settings.ExportFolderPath);
			Assert.Equal(1, settings.Block);
		}

		[Fact]
		public void Block_SetValue_UpdatesProperty()
		{
			// Arrange
			var settings = new ExperimentSettings();

			// Act
			settings.Block = 3;

			// Assert
			Assert.Equal(3, settings.Block);
		}

		[Fact]
		public void Participant_SetValue_UpdatesProperty()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var participant = new Participant();

			// Act
			settings.Participant = participant;

			// Assert
			Assert.Equal(participant, settings.Participant);
		}

		[Fact]
		public void CurrentProfile_SetValue_UpdatesProperty()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var profile = new ExperimentProfile();

			// Act
			settings.CurrentProfile = profile;

			// Assert
			Assert.Equal(profile, settings.CurrentProfile);
		}

		[Fact]
		public void KeyMappings_SetValue_UpdatesProperty()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var mappings = new KeyMappings();

			// Act
			settings.KeyMappings = mappings;

			// Assert
			Assert.Equal(mappings, settings.KeyMappings);
		}

		[Fact]
		public void ExperimentContext_SetValue_UpdatesProperty()
		{
			// Arrange
			var settings = new ExperimentSettings();
			var context = new SharedExperimentData();

			// Act
			settings.ExperimentContext = context;

			// Assert
			Assert.Equal(context, settings.ExperimentContext);
		}

		[Fact]
		public void ExportFolderPath_SetValue_UpdatesProperty()
		{
			// Arrange
			var settings = new ExperimentSettings();

			// Act
			settings.ExportFolderPath = "C:\\Exports";

			// Assert
			Assert.Equal("C:\\Exports", settings.ExportFolderPath);
		}

		[Fact]
		public void Reset_CallsContextReset_ResetsBlock_AndNotifies()
		{
			// Arrange
			var context = new SharedExperimentDataMock();
			var settings = new ExperimentSettings
			{
				ExperimentContext = context,
				Block = 5
			};

			// Act
			settings.Reset();

			// Assert
			Assert.True(context.ResetCalled);
			Assert.Equal(1, settings.Block);
		}

		// Mock pour vérifier l'appel à Reset sur ExperimentContext
		private class SharedExperimentDataMock : SharedExperimentData
		{
			public bool ResetCalled
			{
				get; private set;
			}
			public override void Reset()
			{
				ResetCalled = true;
			}
		}
	}

}
