namespace StroopApp.Models
{
    /// <summary>
    /// Represents a single reaction time data point for a given trial, including validity of the response.
    /// </summary>
    public class ReactionTimePoint
    {
        public int TrialNumber { get; set; }
        public double? ReactionTime { get; set; }
        public bool? IsValidResponse { get; set; }

        public ReactionTimePoint(int trialNumber, double? reactionTime, bool? isValidResponse)
        {
            TrialNumber = trialNumber;
            ReactionTime = reactionTime;
            IsValidResponse = isValidResponse;
        }
    }
}
