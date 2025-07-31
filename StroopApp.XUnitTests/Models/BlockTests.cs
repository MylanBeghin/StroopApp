namespace StroopApp.XUnitTests.Models
{
	using StroopApp.Models;

	using Xunit;

	public class BlockTests
	{
		ExperimentSettings Settings = new ExperimentSettings();
		[Fact]
		public void Constructor_ValidParams_InitializesProperties()
		{
			// Arrange
			var blockNumber = 1;
			var profileName = "user";

			// Act
			var block = new Block(Settings);

			// Assert
			Assert.Equal(blockNumber, block.BlockNumber);
			Assert.Equal(profileName, block._blockExperimentProfile);
			Assert.NotNull(block.TrialRecords);
			Assert.Empty(block.TrialRecords);
		}

		[Fact]
		public void CalculateValues_NoTrials_AllZero()
		{
			// Arrange
			var block = new Block(Settings);

			// Act
			block.CalculateValues();

			// Assert
			Assert.Equal(0, block.TrialsPerBlock);
			Assert.Equal(0, block.Accuracy);
			Assert.Null(block.ResponseTimeMean);
		}

		[Fact]
		public void CalculateValues_AllValidTrials_ReturnsCorrectValues()
		{
			// Arrange
			var block = new Block(Settings);
			block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = 500, Block = 1 });
			block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = 1000, Block = 1 });

			// Act
			block.CalculateValues();

			// Assert
			Assert.Equal(2, block.TrialsPerBlock);
			Assert.Equal(100, block.Accuracy);
			Assert.Equal(750, block.ResponseTimeMean);
		}

		[Fact]
		public void CalculateValues_HalfValidTrials_ReturnsFiftyPercentAccuracy()
		{
			// Arrange
			var block = new Block(Settings);
			block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = 800, Block = 2 });
			block.TrialRecords.Add(new StroopTrial { IsValidResponse = false, ReactionTime = 900, Block = 2 });

			// Act
			block.CalculateValues();

			// Assert
			Assert.Equal(2, block.TrialsPerBlock);
			Assert.Equal(50, block.Accuracy);
			Assert.Equal(850, block.ResponseTimeMean);
		}

		[Fact]
		public void CalculateValues_InvalidResponses_AccuracyZero()
		{
			// Arrange
			var block = new Block(Settings);
			block.TrialRecords.Add(new StroopTrial { IsValidResponse = false, ReactionTime = 500, Block = 3 });
			block.TrialRecords.Add(new StroopTrial { IsValidResponse = false, ReactionTime = 900, Block = 3 });

			// Act
			block.CalculateValues();

			// Assert
			Assert.Equal(2, block.TrialsPerBlock);
			Assert.Equal(0, block.Accuracy);
			Assert.Equal(700, block.ResponseTimeMean);
		}

		[Fact]
		public void CalculateValues_TrialsFromOtherBlock_ResponseTimeIgnoresThem()
		{
			// Arrange
			var block = new Block(Settings);
			block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = 100, Block = 4 });
			block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = 900, Block = 999 }); // autre block

			// Act
			block.CalculateValues();

			// Assert
			Assert.Equal(2, block.TrialsPerBlock);
			Assert.Equal(100, block.Accuracy);
			Assert.Equal(100, block.ResponseTimeMean); // Seul le trial du block courant compte
		}

		[Fact]
		public void CalculateValues_NullReactionTimes_AreIgnoredForMean()
		{
			// Arrange
			var block = new Block(Settings);
			block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = null, Block = 5 });
			block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = 600, Block = 5 });

			// Act
			block.CalculateValues();

			// Assert
			Assert.Equal(2, block.TrialsPerBlock);
			Assert.Equal(100, block.Accuracy);
			Assert.Equal(600, block.ResponseTimeMean);
		}

		[Fact]
		public void CalculateValues_AllNullReactionTimes_ResponseTimeMeanNull()
		{
			// Arrange
			var block = new Block(Settings);
			block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = null, Block = 6 });
			block.TrialRecords.Add(new StroopTrial { IsValidResponse = false, ReactionTime = null, Block = 6 });

			// Act
			block.CalculateValues();

			// Assert
			Assert.Equal(2, block.TrialsPerBlock);
			Assert.Equal(50, block.Accuracy);
			Assert.Null(block.ResponseTimeMean);
		}

		[Fact]
		public void CalculateValues_EmptyCollection_ResponseTimeMeanNull()
		{
			// Arrange
			var block = new Block(Settings);

			// Act
			block.CalculateValues();

			// Assert
			Assert.Equal(0, block.TrialsPerBlock);
			Assert.Equal(0, block.Accuracy);
			Assert.Null(block.ResponseTimeMean);
		}
	}

}
