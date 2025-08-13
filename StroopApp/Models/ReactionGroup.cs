using StroopApp.Core;

public class ReactionGroup : ModelBase
{
	public int StartTrial { get; }
	public int EndTrial { get; }
	public double? Average { get; }
	public int CorrectCount { get; }
	public int TotalCount { get; }

	public string Range => $"{StartTrial} - {EndTrial}";
	public string AverageDisplay => Average.HasValue ? $"{Average.Value:N0} ms" : "NA";
	public string CorrectDisplay => $"{CorrectCount}/{TotalCount}";

	public ReactionGroup(int start, int end, double? average, int correct, int total)
	{
		StartTrial = start;
		EndTrial = end;
		Average = average;
		CorrectCount = correct;
		TotalCount = total;
	}
}