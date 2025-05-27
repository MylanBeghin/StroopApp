using StroopApp.Models;

using Xunit;

namespace StroopApp.XUnitTests.Models
{
	public class ExperimentProfileTests
	{
		[Fact]
		public void UpdateDerivedValues_IncludesAmorceInWordDuration_WhenIsAmorceTrue()
		{
			// Arrange
			var profile = new ExperimentProfile
			{
				CalculationMode = CalculationMode.WordCount,
				FixationDuration = 150,
				MaxReactionTime = 400,
				AmorceDuration = 50,
				IsAmorce = true,
				WordCount = 1
			};

			// Act
			profile.UpdateDerivedValues();

			// Assert
			Assert.Equal(600, profile.WordDuration);
		}

		[Fact]
		public void UpdateDerivedValues_ComputesWordCount_WhenTaskDurationMode()
		{
			// Arrange
			var profile = new ExperimentProfile
			{
				CalculationMode = CalculationMode.TaskDuration,
				Hours = 0,
				Minutes = 1,
				Seconds = 30,
				WordDuration = 500
			};

			// Act
			profile.UpdateDerivedValues();

			// Assert
			Assert.Equal(90_000, profile.TaskDuration);
			Assert.Equal(180, profile.WordCount);
		}

		[Fact]
		public void UpdateDerivedValues_ResetsAmorceAndSwitchPercentage_WhenIsAmorceFalse()
		{
			// Arrange
			var profile = new ExperimentProfile
			{
				IsAmorce = true,
				AmorceDuration = 80,
				SwitchPourcentage = 30
			};

			// Act
			profile.IsAmorce = false;

			// Assert
			Assert.False(profile.IsAmorce);
			Assert.Equal(0, profile.AmorceDuration);
			Assert.Equal(50, profile.SwitchPourcentage);
		}

		[Fact]
		public void UpdateDerivedValues_ComputesTaskDurationAndTimeComponents_WhenWordCountMode()
		{
			// Arrange
			var profile = new ExperimentProfile
			{
				CalculationMode = CalculationMode.WordCount,
				WordCount = 20,
				FixationDuration = 100,
				MaxReactionTime = 150,
				AmorceDuration = 0
			};

			// Act
			profile.UpdateDerivedValues();

			// Assert
			Assert.Equal(5_000, profile.TaskDuration);
			Assert.Equal(0, profile.Hours);
			Assert.Equal(0, profile.Minutes);
			Assert.Equal(5, profile.Seconds);
		}
	}
}
