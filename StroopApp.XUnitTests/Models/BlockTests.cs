using StroopApp.Models;
using Xunit;

namespace StroopApp.XUnitTests.Models;

public class BlockTests
{
    [Fact]
    public void Constructor_ValidParams_InitializesProperties()
    {
        // The default profile name is culture-dependent and the default profile is not an amorce.
        // Arrange
        var settings = new ExperimentSettings();
        var blockNumber = 1;
        var profileName = settings.CurrentProfile.ProfileName;

        // Act
        var block = new Block(settings);

        // Assert
        Assert.Equal(blockNumber, block.BlockNumber);
        Assert.Equal(profileName, block.BlockExperimentProfile);
        Assert.NotNull(block.TrialRecords);
        Assert.Empty(block.TrialRecords);
        Assert.Equal("❎", block.VisualCue);
    }

    [Fact]
    public void CalculateValues_NoTrials_AllZero()
    {
        // No trials should produce zero metrics.
        // Arrange
        var settings = new ExperimentSettings();
        var block = new Block(settings);

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
        // Both trials are valid so accuracy is 100% and the mean is their average.
        // Arrange
        var settings = new ExperimentSettings();
        var block = new Block(settings);
        var currentBlock = block.BlockNumber; // Store for clarity.
        block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = 500, Block = currentBlock });
        block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = 1000, Block = currentBlock });

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
        // One valid and one invalid response should yield 50% accuracy.
        // Arrange
        var settings = new ExperimentSettings();
        var block = new Block(settings);
        var currentBlock = block.BlockNumber;
        block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = 800, Block = currentBlock });
        block.TrialRecords.Add(new StroopTrial { IsValidResponse = false, ReactionTime = 900, Block = currentBlock });

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
        // No valid responses should produce 0% accuracy while still averaging times.
        // Arrange
        var settings = new ExperimentSettings();
        var block = new Block(settings);
        var currentBlock = block.BlockNumber;
        block.TrialRecords.Add(new StroopTrial { IsValidResponse = false, ReactionTime = 500, Block = currentBlock });
        block.TrialRecords.Add(new StroopTrial { IsValidResponse = false, ReactionTime = 900, Block = currentBlock });

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
        // Reaction time mean only includes trials from the current block.
        // Arrange
        var settings = new ExperimentSettings();
        var block = new Block(settings);
        var currentBlock = block.BlockNumber;
        block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = 100, Block = currentBlock });
        block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = 900, Block = 999 }); // other block

        // Act
        block.CalculateValues();

        // Assert
        Assert.Equal(2, block.TrialsPerBlock);
        Assert.Equal(100, block.Accuracy);
        Assert.Equal(100, block.ResponseTimeMean);
    }

    [Fact]
    public void CalculateValues_NullReactionTimes_AreIgnoredForMean()
    {
        // Null reaction times are excluded from the mean calculation.
        // Arrange
        var settings = new ExperimentSettings();
        var block = new Block(settings);
        var currentBlock = block.BlockNumber;
        block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = null, Block = currentBlock });
        block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = 600, Block = currentBlock });

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
        // When all reaction times are null the mean should be null.
        // Arrange
        var settings = new ExperimentSettings();
        var block = new Block(settings);
        var currentBlock = block.BlockNumber;
        block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = null, Block = currentBlock });
        block.TrialRecords.Add(new StroopTrial { IsValidResponse = false, ReactionTime = null, Block = currentBlock });

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
        // Explicitly covering the empty collection scenario.
        // Arrange
        var settings = new ExperimentSettings();
        var block = new Block(settings);

        // Act
        block.CalculateValues();

        // Assert
        Assert.Equal(0, block.TrialsPerBlock);
        Assert.Equal(0, block.Accuracy);
        Assert.Null(block.ResponseTimeMean);
    }

    [Fact]
    public void CalculateValues_NullIsValidResponse_TreatedAsIncorrect()
    {
        // Null validity should be treated as incorrect while still averaging times.
        // Arrange
        var settings = new ExperimentSettings();
        var block = new Block(settings);
        var currentBlock = block.BlockNumber;
        block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = 800, Block = currentBlock });
        block.TrialRecords.Add(new StroopTrial { IsValidResponse = null, ReactionTime = 900, Block = currentBlock });

        // Act
        block.CalculateValues();

        // Assert
        Assert.Equal(2, block.TrialsPerBlock);
        Assert.Equal(50, block.Accuracy); // Only one valid response.
        Assert.Equal(850, block.ResponseTimeMean); // Reaction times are averaged regardless of validity.
    }

    [Fact]
    public void CalculateValues_TrialsFromOtherBlockOnly_ResponseTimeMeanNull()
    {
        // Accuracy counts all trials, but mean is null when none belong to the current block.
        // Arrange
        var settings = new ExperimentSettings();
        var block = new Block(settings);
        block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = 100, Block = 2 });
        block.TrialRecords.Add(new StroopTrial { IsValidResponse = true, ReactionTime = 200, Block = 2 });

        // Act
        block.CalculateValues();

        // Assert
        Assert.Equal(2, block.TrialsPerBlock);
        Assert.Equal(100, block.Accuracy);
        Assert.Null(block.ResponseTimeMean);
    }

    [Fact]
    public void Constructor_AmorceProfile_SetsVisualCueToCheckMark()
    {
        // An amorce profile uses a check mark as its visual cue.
        // Arrange
        var settings = new ExperimentSettings();
        settings.CurrentProfile.IsAmorce = true;

        // Act
        var block = new Block(settings);

        // Assert
        Assert.Equal("✅", block.VisualCue);
        Assert.Equal(settings.CurrentProfile.SwitchPercent, block.SwitchPercent);
    }
}

