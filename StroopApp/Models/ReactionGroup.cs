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
	private int CorrectDisplayValue => (int)System.Math.Round((double)CorrectCount / System.Math.Max(1, TotalCount) * 100);
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