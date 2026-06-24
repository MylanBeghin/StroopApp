namespace StroopApp.Models.Simon
{
    /// <summary>
    /// Represents a single trial in a Stroop task, containing stimulus data, participant responses,
    /// reaction time, and trial metadata such as block and trial number.
    /// </summary>
    public class SimonTrial
    {
        public string ParticipantId { get; set; } = string.Empty;
        public bool IsCongruent { get; set; }
        public bool HasVIsualCue { get; set; }
        public int? SwitchPercent { get; set; }
        public int CongruencePercent { get; set; }
        public int DominancePercent { get; set; }
        public int Block { get; set; }
        public SimonStimulus Stimulus { get; set; } = null!;
        public string ExpectedAnswer { get; set; } = string.Empty;
        public string GivenAnswer { get; set; } = string.Empty;
        public bool? IsValidResponse { get; set; }
        public double? ReactionTime { get; set; }
        public int TrialNumber { get; set; }
        public VisualCueType VisualCue { get; set; }

        /// <summary>
        /// Calculates the expected answer based on trial type (Congruent or Incongruent).
        /// </summary>
        public void DetermineExpectedAnswer()
        {
            if (IsCongruent)
            {
                ExpectedAnswer = Stimulus.Position == StimulusPosition.Left ? "Left" : "Right";
            }
            else
            {
                ExpectedAnswer = Stimulus.Position == StimulusPosition.Left ? "Right" : "Left";
            }
        }
    }
}