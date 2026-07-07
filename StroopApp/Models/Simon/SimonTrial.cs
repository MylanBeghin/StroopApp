namespace StroopApp.Models.Simon
{
    /// <summary>
    /// Represents a single trial in a Stroop task, containing stimulus data, participant responses,
    /// reaction time, and trial metadata such as block and trial number.
    /// </summary>
    public class SimonTrial : ITrial
    {
        public string ParticipantId { get; set; } = string.Empty;
        public bool IsCongruent { get; set; }
        public int? SwitchPercent { get; set; }
        public int CongruencePercent { get; set; }
        public int DominancePercent { get; set; }
        public int Block { get; set; }
        public SimonStimulus Stimulus { get; set; } = null!;
        public SimonAnswer ExpectedAnswer { get; set; }
        public SimonAnswer GivenAnswer { get; set; }
        public bool? IsValidResponse { get; set; }
        public double? ReactionTime { get; set; }
        public int TrialNumber { get; set; }
    }
}