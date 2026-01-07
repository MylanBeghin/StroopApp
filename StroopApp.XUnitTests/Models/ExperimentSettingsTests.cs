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

		// ========== CHARACTERIZATION TESTS FOR Reset() =========
		// These tests freeze the current behavior of Reset() to enable safe refactoring

		[Fact]
		public void Reset_GivenBlockIs5_ResetsBlockTo1()
		{
			// Arrange
			var settings = new ExperimentSettings
			{
				Block = 5
			};

			// Act
			settings.Reset();

			// Assert
			Assert.Equal(1, settings.Block);
		}

		[Fact]
		public void Reset_GivenBlockIs1_BlockRemainsAt1()
		{
			// Arrange
			var settings = new ExperimentSettings
			{
				Block = 1
			};

			// Act
			settings.Reset();

			// Assert
			Assert.Equal(1, settings.Block);
		}

		[Fact]
		public void Reset_AlwaysSetsIsBlockFinishedToTrue()
		{
			// Arrange
			var settings = new ExperimentSettings();
			settings.ExperimentContext.IsBlockFinished = false;

			// Act
			settings.Reset();

			// Assert
			Assert.True(settings.ExperimentContext.IsBlockFinished);
		}

		[Fact]
		public void Reset_AlwaysSetsIsParticipantSelectionEnabledToTrue()
		{
			// Arrange
			var settings = new ExperimentSettings();
			settings.ExperimentContext.IsParticipantSelectionEnabled = false;

			// Act
			settings.Reset();

			// Assert
			Assert.True(settings.ExperimentContext.IsParticipantSelectionEnabled);
		}

		[Fact]
		public void Reset_AlwaysSetsHasUnsavedExportsToTrue()
		{
			// Arrange
			var settings = new ExperimentSettings();
			settings.ExperimentContext.HasUnsavedExports = false;

			// Act
			settings.Reset();

			// Assert
			Assert.True(settings.ExperimentContext.HasUnsavedExports);
		}

		[Fact]
		public void Reset_CallsExperimentContextReset()
		{
			// Arrange
			var spyContext = new SpySharedExperimentData();
			var settings = new ExperimentSettings
			{
				ExperimentContext = spyContext
			};

			// Act
			settings.Reset();

			// Assert
			Assert.Equal(1, spyContext.ResetCallCount);
		}

		[Fact]
		public void Reset_DoesNotModifyParticipant()
		{
			// Arrange
			var participant = new Participant { Id = "TestParticipant123" };
			var settings = new ExperimentSettings
			{
				Participant = participant
			};

			// Act
			settings.Reset();

			// Assert
			Assert.Same(participant, settings.Participant);
			Assert.Equal("TestParticipant123", settings.Participant.Id);
		}

		[Fact]
		public void Reset_DoesNotModifyCurrentProfile()
		{
			// Arrange
			var profile = new ExperimentProfile { ProfileName = "TestProfile" };
			var settings = new ExperimentSettings
			{
				CurrentProfile = profile
			};

			// Act
			settings.Reset();

			// Assert
			Assert.Same(profile, settings.CurrentProfile);
			Assert.Equal("TestProfile", settings.CurrentProfile.ProfileName);
		}

		[Fact]
		public void Reset_DoesNotModifyKeyMappings()
		{
			// Arrange
			var mappings = new KeyMappings();
			var settings = new ExperimentSettings
			{
				KeyMappings = mappings
			};

			// Act
			settings.Reset();

			// Assert
			Assert.Same(mappings, settings.KeyMappings);
		}

		[Fact]
		public void Reset_DoesNotModifyExportFolderPath()
		{
			// Arrange
			var settings = new ExperimentSettings
			{
				ExportFolderPath = "C:\\MyExports"
			};

			// Act
			settings.Reset();

			// Assert
			Assert.Equal("C:\\MyExports", settings.ExportFolderPath);
		}

		[Fact]
		public void Reset_WithComplexStateInExperimentContext_DelegatesResetToContext()
		{
			// Arrange
			var settings = new ExperimentSettings();
			settings.ExperimentContext.CurrentBlock = new Block(settings);
			settings.ExperimentContext.CurrentTrial = new StroopTrial { TrialNumber = 5 };
			settings.ExperimentContext.IsTaskStopped = true;
			settings.ExperimentContext.NextAction = ExperimentAction.Quit;

			// Act
			settings.Reset();

			// Assert - Verify Reset() was called (indirectly via state check)
			// NOTE: CurrentTrial is NOT reset to null by Reset() - this is the observed behavior
			Assert.Null(settings.ExperimentContext.CurrentBlock);
			Assert.NotNull(settings.ExperimentContext.CurrentTrial); // CurrentTrial is NOT reset
			Assert.False(settings.ExperimentContext.IsTaskStopped);
			Assert.Equal(ExperimentAction.None, settings.ExperimentContext.NextAction);
		}

		[Fact]
		public void Reset_RaisesPropertyChangedWithEmptyString()
		{
			// Arrange
			var settings = new ExperimentSettings();
			string capturedPropertyName = null;
			settings.PropertyChanged += (sender, e) => capturedPropertyName = e.PropertyName;

			// Act
			settings.Reset();

			// Assert
			Assert.Equal(string.Empty, capturedPropertyName);
		}

		[Fact]
		public void Reset_ExecutionOrder_ResetsContextBeforeSettingBlock()
		{
			// Arrange
			var orderedContext = new OrderTrackingSharedExperimentData();
			var settings = new ExperimentSettings
			{
				ExperimentContext = orderedContext,
				Block = 10
			};
			
			settings.PropertyChanged += (sender, e) =>
			{
				if (e.PropertyName == string.Empty)
				{
					orderedContext.ExecutionLog.Add("PropertyChanged");
				}
			};

			// Act
			settings.Reset();

			// Assert
			Assert.Contains("Reset", orderedContext.ExecutionLog);
			Assert.Contains("PropertyChanged", orderedContext.ExecutionLog);
			var resetIndex = orderedContext.ExecutionLog.IndexOf("Reset");
			var propertyChangedIndex = orderedContext.ExecutionLog.IndexOf("PropertyChanged");
			Assert.True(resetIndex < propertyChangedIndex, "Reset should be called before PropertyChanged");
		}

		// ========== TEST HELPERS ==========

		private class SharedExperimentDataMock : SharedExperimentData
		{
			public bool ResetCalled { get; private set; }
			
			public override void Reset()
			{
				ResetCalled = true;
			}
		}

		private class SpySharedExperimentData : SharedExperimentData
		{
			public int ResetCallCount { get; private set; }

			public override void Reset()
			{
				ResetCallCount++;
				base.Reset();
			}
		}

		private class OrderTrackingSharedExperimentData : SharedExperimentData
		{
			public List<string> ExecutionLog { get; } = new List<string>();

			public override void Reset()
			{
				ExecutionLog.Add("Reset");
				base.Reset();
			}
		}
	}
}
