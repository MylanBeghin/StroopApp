using StroopApp.Models;

using Xunit;

namespace StroopApp.XUnitTests.Models
{
	public class ExperimentProfileBasicTests
	{
		[Fact]
		public void DefaultConstructor_SetsDefaultValues()
		{
			// Arrange & Act
			var profile = new ExperimentProfile();

			// Assert
			Assert.Equal(50, profile.CongruencePourcentage);
			Assert.Equal(null, profile.SwitchPourcent);
			Assert.False(profile.IsAmorce);
			Assert.Equal(100, profile.FixationDuration);
			Assert.Equal(400, profile.MaxReactionTime);
			Assert.Equal(0, profile.AmorceDuration);
			Assert.Equal(CalculationMode.WordCount, profile.CalculationMode);
			Assert.Equal(10, profile.WordCount);
		}
	}
}
