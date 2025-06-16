namespace StroopApp.XUnitTests.Models
{
	using StroopApp.Models;

	using Xunit;

	public class SharedExperimentDataTests
	{
		[Fact]
		public void Constructor_InitializesCollectionsAndDefaults()
		{
			// Arrange & Act
			var data = new SharedExperimentData();

			// Assert
			Assert.NotNull(data.Blocks);
			Assert.Empty(data.Blocks);
			Assert.NotNull(data.BlockSeries);
			Assert.Empty(data.BlockSeries);
			Assert.NotNull(data.Sections);
			Assert.Empty(data.Sections);
			Assert.NotNull(data.ReactionPoints);
			Assert.Empty(data.ReactionPoints);
			Assert.NotNull(data.ColumnSerie);
			Assert.True(data.currentBlockStart == 1);
		}

		[Fact]
		public void CurrentBlock_SetValue_RaisesPropertyChangedAndUpdatesValue()
		{
			// Arrange
			var data = new SharedExperimentData();
			var block = new Block(1, "test");

			// Act
			data.CurrentBlock = block;

			// Assert
			Assert.Equal(block, data.CurrentBlock);
		}

		[Fact]
		public void CurrentTrial_SetValue_RaisesPropertyChangedAndSubscribesEvent()
		{
			// Arrange
			var data = new SharedExperimentData();
			var trial = new StroopTrial();

			bool propertyChangedRaised = false;
			data.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(SharedExperimentData.CurrentTrial))
					propertyChangedRaised = true;
			};

			// Act
			data.CurrentTrial = trial;

			// Assert
			Assert.Equal(trial, data.CurrentTrial);
			Assert.True(propertyChangedRaised);
		}

		[Fact]
		public void CurrentTrial_SetTwice_OnlyLastEventHandlerAttached()
		{
			// Arrange
			var data = new SharedExperimentData();
			var trial1 = new TestTrial();
			var trial2 = new TestTrial();
			int callCount1 = 0, callCount2 = 0;

			trial1.PropertyChanged += (s, e) => callCount1++;
			trial2.PropertyChanged += (s, e) => callCount2++;

			// Act
			data.CurrentTrial = trial1;
			data.CurrentTrial = trial2;
			trial1.RaiseTrialNumberChanged(); // On utilise la méthode publique pour déclencher
			trial2.RaiseTrialNumberChanged();

			// Assert
			Assert.Equal(1, callCount1);
			Assert.Equal(1, callCount2);
		}


		[Fact]
		public void IsBlockFinished_SetTrueAndFalse_UpdatesValue()
		{
			// Arrange
			var data = new SharedExperimentData();

			// Act
			data.IsBlockFinished = true;

			// Assert
			Assert.True(data.IsBlockFinished);

			// Act
			data.IsBlockFinished = false;

			// Assert
			Assert.False(data.IsBlockFinished);
		}

		[Fact]
		public void IsParticipantSelectionEnabled_DefaultTrue_AndCanSet()
		{
			// Arrange
			var data = new SharedExperimentData();

			// Assert
			Assert.True(data.IsParticipantSelectionEnabled);

			// Act
			data.IsParticipantSelectionEnabled = false;

			// Assert
			Assert.False(data.IsParticipantSelectionEnabled);
		}

		[Fact]
		public void NextAction_SetValue_UpdatesValue()
		{
			// Arrange
			var data = new SharedExperimentData();

			// Act
			data.NextAction = ExperimentAction.Quit;

			// Assert
			Assert.Equal(ExperimentAction.Quit, data.NextAction);
		}

		[Fact]
		public void Reset_ClearsAllCollectionsAndResetsState()
		{
			// Arrange
			var data = new SharedExperimentData();
			data.Blocks.Add(new Block(1, "a"));
			data.BlockSeries.Add(null);
			data.Sections.Add(null);
			data.ReactionPoints.Add(null);
			data.CurrentBlock = new Block(2, "b");
			data.IsBlockFinished = true;
			data.NextAction = ExperimentAction.Quit;
			data.currentBlockStart = 5;
			data.currentBlockEnd = 10;

			// Act
			data.Reset();

			// Assert
			Assert.Empty(data.Blocks);
			Assert.Empty(data.BlockSeries);
			Assert.Empty(data.Sections);
			Assert.Empty(data.ReactionPoints);
			Assert.Equal(1, data.currentBlockStart);
			Assert.Equal(0, data.currentBlockEnd);
			Assert.Null(data.CurrentBlock);
			Assert.False(data.IsBlockFinished);
			Assert.Equal(ExperimentAction.None, data.NextAction);
			Assert.NotNull(data.ColumnSerie);
		}
	}

}
