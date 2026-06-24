namespace StroopApp.Models.Simon
{
    public class SimonStimulus
    {
        /// <summary>
        /// Color name associated with the word (e.g., "Red", "Blue").
        /// </summary>
        public string Color { get; set; } = null!;

        /// <summary>
        /// Internal/semantic text of the word (e.g., "Left").
        /// </summary>
        public StimulusPosition Position { get; set; }

        /// <summary>
        /// Initializes a circle stimulus with color and position.
        /// </summary>
        public SimonStimulus(string color, StimulusPosition position, string displayedText)
        {
            Color = color;
            Position = position;
        }
    }
}
