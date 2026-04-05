using StroopApp.Models;
using System;
using Xunit;


namespace StroopApp.Models.UnitTests
{
    /// <summary>
    /// Unit tests for the StroopTrial class.
    /// </summary>
    public partial class StroopTrialTests
    {
        /// <summary>
        /// Tests that DetermineExpectedAnswer sets ExpectedAnswer to Stimulus.InternalText
        /// when HasVIsualCue is true and VisualCue is Square.
        /// </summary>
        [Fact]
        public void DetermineExpectedAnswer_HasVisualCueAndVisualCueIsSquare_SetsExpectedAnswerToInternalText()
        {
            // Arrange
            var trial = new StroopTrial
            {
                HasVIsualCue = true,
                VisualCue = VisualCueType.Square,
                Stimulus = new Word("Blue", "Red", "Red")
            };

            // Act
            trial.DetermineExpectedAnswer();

            // Assert
            Assert.Equal("Red", trial.ExpectedAnswer);
        }

        /// <summary>
        /// Tests that DetermineExpectedAnswer sets ExpectedAnswer to Stimulus.Color
        /// when HasVIsualCue is true and VisualCue is Round.
        /// </summary>
        [Fact]
        public void DetermineExpectedAnswer_HasVisualCueAndVisualCueIsRound_SetsExpectedAnswerToColor()
        {
            // Arrange
            var trial = new StroopTrial
            {
                HasVIsualCue = true,
                VisualCue = VisualCueType.Round,
                Stimulus = new Word("Blue", "Red", "Red")
            };

            // Act
            trial.DetermineExpectedAnswer();

            // Assert
            Assert.Equal("Blue", trial.ExpectedAnswer);
        }

        /// <summary>
        /// Tests that DetermineExpectedAnswer sets ExpectedAnswer to Stimulus.Color
        /// when HasVIsualCue is true and VisualCue is None.
        /// </summary>
        [Fact]
        public void DetermineExpectedAnswer_HasVisualCueAndVisualCueIsNone_SetsExpectedAnswerToColor()
        {
            // Arrange
            var trial = new StroopTrial
            {
                HasVIsualCue = true,
                VisualCue = VisualCueType.None,
                Stimulus = new Word("Green", "Yellow", "Yellow")
            };

            // Act
            trial.DetermineExpectedAnswer();

            // Assert
            Assert.Equal("Green", trial.ExpectedAnswer);
        }

        /// <summary>
        /// Tests that DetermineExpectedAnswer sets ExpectedAnswer to Stimulus.InternalText
        /// when HasVIsualCue is false and IsCongruent is true.
        /// </summary>
        [Fact]
        public void DetermineExpectedAnswer_NoVisualCueAndIsCongruent_SetsExpectedAnswerToInternalText()
        {
            // Arrange
            var trial = new StroopTrial
            {
                HasVIsualCue = false,
                IsCongruent = true,
                Stimulus = new Word("Blue", "Red", "Red")
            };

            // Act
            trial.DetermineExpectedAnswer();

            // Assert
            Assert.Equal("Red", trial.ExpectedAnswer);
        }

        /// <summary>
        /// Tests that DetermineExpectedAnswer sets ExpectedAnswer to Stimulus.Color
        /// when HasVIsualCue is false and IsCongruent is false (incongruent trial).
        /// </summary>
        [Fact]
        public void DetermineExpectedAnswer_NoVisualCueAndIsIncongruent_SetsExpectedAnswerToColor()
        {
            // Arrange
            var trial = new StroopTrial
            {
                HasVIsualCue = false,
                IsCongruent = false,
                Stimulus = new Word("Blue", "Red", "Red")
            };

            // Act
            trial.DetermineExpectedAnswer();

            // Assert
            Assert.Equal("Blue", trial.ExpectedAnswer);
        }

        /// <summary>
        /// Tests that DetermineExpectedAnswer handles empty string values for Color and InternalText
        /// by setting ExpectedAnswer to empty string when appropriate.
        /// </summary>
        [Theory]
        [InlineData(true, VisualCueType.Square, "", "")]
        [InlineData(true, VisualCueType.Round, "", "")]
        [InlineData(false, true, "", "")]
        [InlineData(false, false, "", "")]
        public void DetermineExpectedAnswer_EmptyColorOrInternalText_SetsExpectedAnswerToEmptyString(
            bool hasVisualCue, object visualCueOrCongruent, string color, string internalText)
        {
            // Arrange
            var trial = new StroopTrial
            {
                HasVIsualCue = hasVisualCue,
                Stimulus = new Word(color, internalText, "Display")
            };

            if (hasVisualCue)
            {
                trial.VisualCue = (VisualCueType)visualCueOrCongruent;
            }
            else
            {
                trial.IsCongruent = (bool)visualCueOrCongruent;
            }

            // Act
            trial.DetermineExpectedAnswer();

            // Assert
            Assert.Equal("", trial.ExpectedAnswer);
        }

        /// <summary>
        /// Tests that DetermineExpectedAnswer correctly prioritizes HasVIsualCue over IsCongruent
        /// when both are true.
        /// </summary>
        [Fact]
        public void DetermineExpectedAnswer_BothHasVisualCueAndIsCongruentTrue_PrioritizesVisualCue()
        {
            // Arrange
            var trial = new StroopTrial
            {
                HasVIsualCue = true,
                IsCongruent = true,
                VisualCue = VisualCueType.Round,
                Stimulus = new Word("Blue", "Red", "Red")
            };

            // Act
            trial.DetermineExpectedAnswer();

            // Assert
            Assert.Equal("Blue", trial.ExpectedAnswer);
        }
    }
}