namespace StroopApp.Models
{
    /// <summary>
    /// Represents aggregated statistics for a group of trials (reaction times, accuracy) over a specific range.
    /// </summary>
    public class ReactionGroup
    {
        public int StartTrial { get; }
        public int EndTrial { get; }
        public double? Average { get; }
        public int CorrectCount { get; }
        public int TotalCount { get; }

        public string Range => $"{StartTrial} - {EndTrial}";
        public string AverageDisplay => Average.HasValue ? $"{Average.Value:N0} ms" : "NA";
        private int CorrectDisplayValue => (int)Math.Round((double)CorrectCount / Math.Max(1, TotalCount) * 100);
        public string CorrectDisplay => $"{CorrectDisplayValue} %";

        public ReactionGroup(int start, int end, double? average, int correct, int total)
        {
            StartTrial = start;
            EndTrial = end;
            Average = average;
            CorrectCount = correct;
            TotalCount = total;
        }
    }
}