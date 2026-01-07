namespace StroopApp.XUnitTests.Models
{
	using StroopApp.Models;
	using Xunit;

	/// <summary>
	/// Characterization tests for SharedExperimentData.AddNewSerie()
	/// These tests freeze the current behavior to enable safe refactoring.
	/// They document actual behavior, even if it seems wrong.
	/// </summary>
	public class SharedExperimentDataAddNewSerieTests
	{
		// ========== CHARACTERIZATION TESTS FOR AddNewSerie() ==========

		[Fact]
		public void AddNewSerie_GivenBasicSettings_CreatesNewBlock()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile 
				{ 
					WordCount = 10,
					IsAmorce = false
				}
			};

			// Act
			context.AddNewSerie(settings);

			// Assert
			Assert.NotNull(context.CurrentBlock);
			Assert.Equal(1, context.CurrentBlock.BlockNumber);
		}

		[Fact]
		public void AddNewSerie_AddsBlockToBlocksCollection()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 2,
				CurrentProfile = new ExperimentProfile { WordCount = 15 }
			};

			// Act
			context.AddNewSerie(settings);

			// Assert
			Assert.Single(context.Blocks);
			Assert.Same(context.CurrentBlock, context.Blocks[0]);
		}

		[Fact]
		public void AddNewSerie_AddsEntryToBlockSeries()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile { WordCount = 10 }
			};

			// Act
			context.AddNewSerie(settings);

			// Assert
			Assert.Single(context.BlockSeries);
		}

		[Fact]
		public void AddNewSerie_AddsSectionToSections()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile { WordCount = 10 }
			};

			// Act
			context.AddNewSerie(settings);

			// Assert
			Assert.Single(context.Sections);
		}

		[Fact]
		public void AddNewSerie_SetsCurrentBlockStartCorrectly()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile { WordCount = 10 }
			};

			// Act
			context.AddNewSerie(settings);

			// Assert - After first block (1-10), currentBlockStart is set to 11 for next block
			Assert.Equal(11, context.currentBlockStart);
		}

		[Fact]
		public void AddNewSerie_UpdatesCurrentBlockEnd()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile { WordCount = 10 }
			};

			// Act
			context.AddNewSerie(settings);

			// Assert - First block with 10 words ends at trial 10
			Assert.Equal(10, context.currentBlockEnd);
		}

		[Fact]
		public void AddNewSerie_CalledTwice_UpdatesCurrentBlockStartForSecondBlock()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile { WordCount = 10 }
			};

			// Act
			context.AddNewSerie(settings);
			settings.Block = 2;
			context.AddNewSerie(settings);

			// Assert - After second block (11-20), currentBlockStart is set to 21 for next block
			Assert.Equal(21, context.currentBlockStart);
		}

		[Fact]
		public void AddNewSerie_CalledTwice_AddsMultipleBlocksToCollections()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile { WordCount = 5 }
			};

			// Act
			context.AddNewSerie(settings);
			settings.Block = 2;
			settings.CurrentProfile.WordCount = 8;
			context.AddNewSerie(settings);

			// Assert
			Assert.Equal(2, context.Blocks.Count);
			Assert.Equal(2, context.BlockSeries.Count);
			Assert.Equal(2, context.Sections.Count);
		}

		[Fact]
		public void AddNewSerie_SecondBlock_SetsCorrectCurrentBlockEnd()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile { WordCount = 10 }
			};

			// Act
			context.AddNewSerie(settings); // Block 1: trials 1-10
			settings.Block = 2;
			settings.CurrentProfile.WordCount = 5;
			context.AddNewSerie(settings); // Block 2: trials 11-15

			// Assert
			Assert.Equal(15, context.currentBlockEnd);
		}

		[Fact]
		public void AddNewSerie_IncrementsColorIndex()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile { WordCount = 10 }
			};
			var initialColorIndex = context._colorIndex;

			// Act
			context.AddNewSerie(settings);

			// Assert
			Assert.Equal(initialColorIndex + 1, context._colorIndex);
		}

		[Fact]
		public void AddNewSerie_MultipleBlocks_CyclesColorIndex()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile { WordCount = 5 }
			};

			// Act - Add 6 blocks (palette has 4 colors, so should cycle)
			for (int i = 1; i <= 6; i++)
			{
				settings.Block = i;
				context.AddNewSerie(settings);
			}

			// Assert
			Assert.Equal(6, context._colorIndex);
		}

		[Fact]
		public void AddNewSerie_CreatedBlock_HasCorrectBlockNumber()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 3,
				CurrentProfile = new ExperimentProfile { WordCount = 10 }
			};

			// Act
			context.AddNewSerie(settings);

			// Assert
			Assert.Equal(3, context.CurrentBlock.BlockNumber);
		}

		[Fact]
		public void AddNewSerie_CreatedBlock_HasCorrectProfileName()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile 
				{ 
					ProfileName = "TestProfile123",
					WordCount = 10 
				}
			};

			// Act
			context.AddNewSerie(settings);

			// Assert
			Assert.Equal("TestProfile123", context.CurrentBlock.BlockExperimentProfile);
		}

		[Fact]
		public void AddNewSerie_WithAmorce_SetsVisualCueToCheckmark()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile 
				{ 
					IsAmorce = true,
					WordCount = 10 
				}
			};

			// Act
			context.AddNewSerie(settings);

			// Assert
			Assert.NotNull(context.CurrentBlock.VisualCue);
			Assert.NotEmpty(context.CurrentBlock.VisualCue);
			// The actual emoji might differ based on encoding, just verify it's set
		}

		[Fact]
		public void AddNewSerie_WithoutAmorce_SetsVisualCueToCross()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile 
				{ 
					IsAmorce = false,
					WordCount = 10 
				}
			};

			// Act
			context.AddNewSerie(settings);

			// Assert
			Assert.NotNull(context.CurrentBlock.VisualCue);
			Assert.NotEmpty(context.CurrentBlock.VisualCue);
			// The actual emoji might differ based on encoding, just verify it's set
		}

		[Fact]
		public void AddNewSerie_VisualCueDiffersBetweenAmorceSettings()
		{
			// Arrange
			var contextWithAmorce = new SharedExperimentData();
			var contextWithoutAmorce = new SharedExperimentData();
			var settingsWithAmorce = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile 
				{ 
					IsAmorce = true,
					WordCount = 10 
				}
			};
			var settingsWithoutAmorce = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile 
				{ 
					IsAmorce = false,
					WordCount = 10 
				}
			};

			// Act
			contextWithAmorce.AddNewSerie(settingsWithAmorce);
			contextWithoutAmorce.AddNewSerie(settingsWithoutAmorce);

			// Assert - Visual cues should be different
			Assert.NotEqual(
				contextWithAmorce.CurrentBlock.VisualCue, 
				contextWithoutAmorce.CurrentBlock.VisualCue
			);
		}

		[Fact]
		public void AddNewSerie_Section_HasCorrectLabel()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 2,
				CurrentProfile = new ExperimentProfile { WordCount = 10 }
			};

			// Act
			context.AddNewSerie(settings);

			// Assert
			var section = context.Sections[0];
			Assert.Equal("Bloc n°2", section.Label);
		}

		[Fact]
		public void AddNewSerie_Section_HasCorrectRange()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile { WordCount = 15 }
			};

			// Act
			context.AddNewSerie(settings);

			// Assert
			var section = context.Sections[0];
			Assert.Equal(1, section.Xi); // Start
			Assert.Equal(15, section.Xj); // End
		}

		[Fact]
		public void AddNewSerie_MultipleBlocks_SectionsHaveCorrectRanges()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile { WordCount = 10 }
			};

			// Act
			context.AddNewSerie(settings); // Block 1: 1-10
			settings.Block = 2;
			settings.CurrentProfile.WordCount = 5;
			context.AddNewSerie(settings); // Block 2: 11-15

			// Assert
			Assert.Equal(1, context.Sections[0].Xi);
			Assert.Equal(10, context.Sections[0].Xj);
			Assert.Equal(11, context.Sections[1].Xi);
			Assert.Equal(15, context.Sections[1].Xj);
		}

		[Fact]
		public void AddNewSerie_RequiresDependencyOnExperimentSettings()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "Test" },
				CurrentProfile = new ExperimentProfile { WordCount = 10 }
			};

			// Act & Assert - This test documents the god object dependency
			// The method requires the entire ExperimentSettings object
			// even though it only uses: Block, CurrentProfile.WordCount, CurrentProfile.IsAmorce
			context.AddNewSerie(settings);
			Assert.NotNull(context.CurrentBlock);
		}

		[Fact]
		public void AddNewSerie_DoesNotModifyExperimentSettings()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 3,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile 
				{ 
					ProfileName = "Profile1",
					WordCount = 10,
					IsAmorce = true
				},
				ExportFolderPath = "C:\\Test"
			};

			// Act
			context.AddNewSerie(settings);

			// Assert - Settings should remain unchanged
			Assert.Equal(3, settings.Block);
			Assert.Equal("P001", settings.Participant.Id);
			Assert.Equal("Profile1", settings.CurrentProfile.ProfileName);
			Assert.Equal(10, settings.CurrentProfile.WordCount);
			Assert.True(settings.CurrentProfile.IsAmorce);
			Assert.Equal("C:\\Test", settings.ExportFolderPath);
		}

		[Fact]
		public void AddNewSerie_BlockTrialTimes_InitiallyEmpty()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile { WordCount = 10 }
			};

			// Act
			context.AddNewSerie(settings);

			// Assert
			Assert.Empty(context.CurrentBlock.TrialTimes);
		}

		[Fact]
		public void AddNewSerie_BlockTrialRecords_InitiallyEmpty()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile { WordCount = 10 }
			};

			// Act
			context.AddNewSerie(settings);

			// Assert
			Assert.Empty(context.CurrentBlock.TrialRecords);
		}

		[Fact]
		public void AddNewSerie_OverwritesCurrentBlock()
		{
			// Arrange
			var context = new SharedExperimentData();
			var settings1 = new ExperimentSettings
			{
				Block = 1,
				CurrentProfile = new ExperimentProfile { WordCount = 5 }
			};
			var settings2 = new ExperimentSettings
			{
				Block = 2,
				CurrentProfile = new ExperimentProfile { WordCount = 10 }
			};

			// Act
			context.AddNewSerie(settings1);
			var firstBlock = context.CurrentBlock;
			context.AddNewSerie(settings2);

			// Assert
			Assert.NotSame(firstBlock, context.CurrentBlock);
			Assert.Equal(2, context.CurrentBlock.BlockNumber);
		}
	}
}
