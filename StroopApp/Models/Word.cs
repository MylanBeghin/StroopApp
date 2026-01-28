namespace StroopApp.Models
{
	/// <summary>
	/// Represents a Stroop stimulus with a displayed text and an associated color.
	/// </summary>
	public class Word
	{
		public string Color { get; set; }           // ex: "Red"
	public string Text { get; set; }
	public string InternalText { get; set; }

		public Word(string color, string internalText, string displayedText)
		{
			Color = color;
			InternalText = internalText;
			Text = displayedText;
		}
	}
}
