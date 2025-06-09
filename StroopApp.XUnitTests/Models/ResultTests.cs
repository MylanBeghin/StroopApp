using StroopApp.Models;

using Xunit;

namespace StroopApp.XUnitTests.Models
{
	public class ResultTests
	{
		[Fact]
		public void Constructor_InitializesPropertiesWithDefaults()
		{
			// Arrange & Act
			var result = new Result();

			// Assert
			Assert.Equal(0, result.ParticipantId);
			Assert.Equal(string.Empty, result.StroopType);
			Assert.Equal(1, result.Block);
			Assert.Equal(string.Empty, result.ExpectedResponse);
			Assert.Equal(string.Empty, result.GivenResponse);
			Assert.Equal(0, result.ReactionTime);
			Assert.Equal(0, result.TrialNumber);
			Assert.Equal(AmorceType.None, result.Amorce);
			Assert.True(result.IsCorrect); // Deux strings vides = true
		}

		[Fact]
		public void IsCorrect_ReturnsTrue_WhenResponsesMatch_IgnoreCase()
		{
			// Arrange
			var result = new Result
			{
				ExpectedResponse = "Rouge",
				GivenResponse = "rouge"
			};

			// Act & Assert
			Assert.True(result.IsCorrect);
		}

		[Fact]
		public void IsCorrect_ReturnsFalse_WhenResponsesDiffer()
		{
			// Arrange
			var result = new Result
			{
				ExpectedResponse = "Jaune",
				GivenResponse = "Vert"
			};

			// Act & Assert
			Assert.False(result.IsCorrect);
		}

		[Fact]
		public void SettingExpectedResponse_RaisesIsCorrectPropertyChanged()
		{
			// Arrange
			var result = new Result();
			bool isCorrectChanged = false;
			result.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(Result.IsCorrect))
					isCorrectChanged = true;
			};

			// Act
			result.ExpectedResponse = "Test";

			// Assert
			Assert.True(isCorrectChanged);
		}

		[Fact]
		public void SettingGivenResponse_RaisesIsCorrectPropertyChanged()
		{
			// Arrange
			var result = new Result();
			bool isCorrectChanged = false;
			result.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(Result.IsCorrect))
					isCorrectChanged = true;
			};

			// Act
			result.GivenResponse = "Test";

			// Assert
			Assert.True(isCorrectChanged);
		}

		[Fact]
		public void AllSetters_RaisePropertyChanged()
		{
			// Arrange
			var result = new Result();
			var changedProps = new System.Collections.Generic.List<string>();
			result.PropertyChanged += (s, e) => changedProps.Add(e.PropertyName);

			// Act
			result.ParticipantId = 42;
			result.StroopType = "TestType";
			result.Block = 3;
			result.ExpectedResponse = "A";
			result.GivenResponse = "B";
			result.ReactionTime = 123;
			result.TrialNumber = 10;
			result.Amorce = AmorceType.Round;

			// Assert
			Assert.Contains(nameof(Result.ParticipantId), changedProps);
			Assert.Contains(nameof(Result.StroopType), changedProps);
			Assert.Contains(nameof(Result.Block), changedProps);
			Assert.Contains(nameof(Result.ExpectedResponse), changedProps);
			Assert.Contains(nameof(Result.GivenResponse), changedProps);
			Assert.Contains(nameof(Result.ReactionTime), changedProps);
			Assert.Contains(nameof(Result.TrialNumber), changedProps);
			Assert.Contains(nameof(Result.Amorce), changedProps);
		}
	}
}
