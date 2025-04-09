namespace StroopApp.Models
{
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
