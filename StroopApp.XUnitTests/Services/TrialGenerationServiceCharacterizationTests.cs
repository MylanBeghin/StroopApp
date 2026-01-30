namespace StroopApp.XUnitTests.Services
{
	using StroopApp.Models;
	using StroopApp.Services.Language;
	using StroopApp.Services.Trial;
	using StroopApp.XUnitTests.TestDummies;
	using Xunit;

	/// <summary>
	/// Characterization tests for TrialGenerationService.GenerateTrials()
	/// These tests freeze the current behavior to enable safe refactoring.
	/// </summary>
	public class TrialGenerationServiceCharacterizationTests
	{
		private readonly ILanguageService _languageService;

		public TrialGenerationServiceCharacterizationTests()
		{
			_languageService = new DummyLanguageService();
		}

		// ========== BASIC GENERATION ==========

		[Fact]
		public void GenerateTrials_GivenBasicSettings_ReturnsCorrectTrialCount()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile
				{
					WordCount = 10,
					CongruencePercent = 50,
					IsAmorce = false
				}
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.Equal(10, trials.Count);
		}

		[Fact]
		public void GenerateTrials_GeneratesSequentialTrialNumbers()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile { WordCount = 5 }
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			for (int i = 0; i < trials.Count; i++)
			{
				Assert.Equal(i + 1, trials[i].TrialNumber);
			}
		}

		[Fact]
		public void GenerateTrials_AllTrialsHaveCorrectBlockNumber()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 3,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile { WordCount = 8 }
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.All(trials, trial => Assert.Equal(3, trial.Block));
		}

		[Fact]
		public void GenerateTrials_AllTrialsHaveCorrectParticipantId()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P123" },
				CurrentProfile = new ExperimentProfile { WordCount = 5 }
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.All(trials, trial => Assert.Equal("P123", trial.ParticipantId));
		}

		// ========== CONGRUENCE TESTS ==========

		[Fact]
		public void GenerateTrials_50PercentCongruence_GeneratesApproximatelyHalfCongruent()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile
				{
					WordCount = 100,
					CongruencePercent = 50,
					IsAmorce = false
				}
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			var congruentCount = trials.Count(t => t.IsCongruent);
			Assert.Equal(50, congruentCount);
		}

		[Fact]
		public void GenerateTrials_80PercentCongruence_Generates80Congruent()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile
				{
					WordCount = 100,
					CongruencePercent = 80,
					IsAmorce = false
				}
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			var congruentCount = trials.Count(t => t.IsCongruent);
			Assert.Equal(80, congruentCount);
		}

		[Fact]
		public void GenerateTrials_20PercentCongruence_Generates20Congruent()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile
				{
					WordCount = 100,
					CongruencePercent = 20,
					IsAmorce = false
				}
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			var congruentCount = trials.Count(t => t.IsCongruent);
			Assert.Equal(20, congruentCount);
		}

		[Fact]
		public void GenerateTrials_CongruentTrial_HasMatchingTextAndColor()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile
				{
					WordCount = 100,
					CongruencePercent = 100, // All congruent
					IsAmorce = false
				}
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.All(trials, trial =>
			{
				Assert.True(trial.IsCongruent);
				Assert.Equal(trial.Stimulus.Color, trial.Stimulus.InternalText);
			});
		}

		[Fact]
		public void GenerateTrials_IncongruentTrial_HasDifferentTextAndColor()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile
				{
					WordCount = 100,
					CongruencePercent = 0, // All incongruent
					IsAmorce = false
				}
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.All(trials, trial =>
			{
				Assert.False(trial.IsCongruent);
				Assert.NotEqual(trial.Stimulus.Color, trial.Stimulus.InternalText);
			});
		}

		// ========== AMORCE TESTS ==========

		[Fact]
		public void GenerateTrials_WithoutAmorce_IsAmorceFalse()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
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
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.All(trials, trial => Assert.False(trial.IsAmorce));
		}

		[Fact]
		public void GenerateTrials_WithAmorce_IsAmorceTrue()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile
				{
					WordCount = 10,
					IsAmorce = true,
					DominantPercent = 50
				}
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.All(trials, trial => Assert.True(trial.IsAmorce));
		}

		[Fact]
		public void GenerateTrials_WithAmorce_AllTrialsHaveAmorceType()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile
				{
					WordCount = 10,
					IsAmorce = true,
					DominantPercent = 50
				}
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.All(trials, trial =>
			{
				Assert.True(trial.VisualCue == VisualCueType.Round || trial.VisualCue == VisualCueType.Square);
			});
		}

		// ========== EXPECTED ANSWER TESTS ==========

		[Fact]
		public void GenerateTrials_SetsExpectedAnswerForAllTrials()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile
				{
					WordCount = 20,
					IsAmorce = false
				}
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.All(trials, trial =>
			{
				Assert.NotNull(trial.ExpectedAnswer);
				Assert.NotEmpty(trial.ExpectedAnswer);
			});
		}

		[Fact]
		public void GenerateTrials_CongruentWithoutAmorce_ExpectedAnswerMatchesText()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile
				{
					WordCount = 50,
					CongruencePercent = 100,
					IsAmorce = false
				}
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.All(trials, trial =>
			{
				Assert.Equal(trial.Stimulus.InternalText, trial.ExpectedAnswer);
			});
		}

		[Fact]
		public void GenerateTrials_IncongruentWithoutAmorce_ExpectedAnswerMatchesColor()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile
				{
					WordCount = 50,
					CongruencePercent = 0,
					IsAmorce = false
				}
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.All(trials, trial =>
			{
				Assert.Equal(trial.Stimulus.Color, trial.ExpectedAnswer);
			});
		}

		// ========== STIMULUS TESTS ==========

		[Fact]
		public void GenerateTrials_AllTrialsHaveStimulus()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile { WordCount = 10 }
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.All(trials, trial =>
			{
				Assert.NotNull(trial.Stimulus);
				Assert.NotNull(trial.Stimulus.Color);
				Assert.NotNull(trial.Stimulus.InternalText);
				Assert.NotNull(trial.Stimulus.Text);
			});
		}

		[Fact]
		public void GenerateTrials_UsesOnlyValidColors()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile { WordCount = 100 }
			};
			var validColors = new[] { "Blue", "Red", "Green", "Yellow" };

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.All(trials, trial =>
			{
				Assert.Contains(trial.Stimulus.Color, validColors);
				Assert.Contains(trial.Stimulus.InternalText, validColors);
			});
		}

		// ========== PROFILE PROPERTY PROPAGATION ==========

		[Fact]
		public void GenerateTrials_SetsCongruencePercentOnTrials()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile
				{
					WordCount = 10,
					CongruencePercent = 75,
					IsAmorce = false
				}
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.All(trials, trial => Assert.Equal(75, trial.CongruencePercent));
		}

		[Fact]
		public void GenerateTrials_WithAmorce_SetsSwitchPercentOnTrials()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile
				{
					WordCount = 10,
					IsAmorce = true,
					DominantPercent = 60
				}
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.All(trials, trial => Assert.Equal(60, trial.SwitchPercent));
		}

		// ========== EDGE CASES ==========

		[Fact]
		public void GenerateTrials_WithOneWord_GeneratesOneTrial()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile
				{
					WordCount = 1,
					CongruencePercent = 100
				}
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.Single(trials);
			Assert.Equal(1, trials[0].TrialNumber);
		}

		[Fact]
		public void GenerateTrials_LargeWordCount_GeneratesCorrectCount()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = new ExperimentProfile { WordCount = 500 }
			};

			// Act
			var trials = service.GenerateTrials(settings);

			// Assert
			Assert.Equal(500, trials.Count);
		}

		// ========== DEPENDENCY VERIFICATION ==========

		[Fact]
		public void GenerateTrials_RequiresExperimentSettings()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);

			// Act & Assert - Documents the god object dependency
			// Method requires entire ExperimentSettings even though it only needs:
			// - CurrentProfile (WordCount, CongruencePercent, IsAmorce, DominantPercent)
			// - Participant.Id
			// - Block
			Assert.Throws<ArgumentException>(() => service.GenerateTrials((ExperimentSettings)null));
		}

		[Fact]
		public void GenerateTrials_RequiresCurrentProfile()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var settings = new ExperimentSettings
			{
				Block = 1,
				Participant = new Participant { Id = "P001" },
				CurrentProfile = null
			};

			// Act & Assert
			Assert.Throws<ArgumentException>(() => service.GenerateTrials(settings));
		}

		[Fact]
		public void GenerateTrials_DoesNotModifySettings()
		{
			// Arrange
			var service = new TrialGenerationService(_languageService);
			var originalProfile = new ExperimentProfile
			{
				WordCount = 10,
				CongruencePercent = 50,
				IsAmorce = false
			};
			var settings = new ExperimentSettings
			{
				Block = 2,
				Participant = new Participant { Id = "P123" },
				CurrentProfile = originalProfile
			};

			// Act
			service.GenerateTrials(settings);

			// Assert - Settings should remain unchanged
			Assert.Equal(2, settings.Block);
			Assert.Equal("P123", settings.Participant.Id);
			Assert.Same(originalProfile, settings.CurrentProfile);
			Assert.Equal(10, settings.CurrentProfile.WordCount);
			Assert.Equal(50, settings.CurrentProfile.CongruencePercent);
			Assert.False(settings.CurrentProfile.IsAmorce);
		}
	}
}
