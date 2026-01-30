namespace StroopApp.Models
{
    /// <summary>
    /// Represents a Stroop stimulus with a displayed text and an associated color.
    /// </summary>
    public class Word
    {
        /// <summary>
        /// Color name associated with the word (e.g., "Red", "Blue").
        /// </summary>
        public string Color { get; set; } = null!;

        /// <summary>
        /// Internal/semantic text of the word (e.g., "Red").
        /// </summary>
        public string InternalText { get; set; } = null!;

        /// <summary>
        /// Displayed text shown to the participant (may differ from InternalText for translations).
        /// </summary>
        public string Text { get; set; } = null!;

        /// <summary>
        /// Initializes a word stimulus with color, internal text, and display text.
        /// </summary>
        public Word(string color, string internalText, string displayedText)
        {
            Color = color;
            InternalText = internalText;
            Text = displayedText;
        }
    }
}
