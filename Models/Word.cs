namespace StroopApp.Models
{
    /// <summary>
    /// Represents a Stroop stimulus with a displayed text and an associated color.
    /// </summary>
    public class Word
    {
        public string Color { get; set; }

        public string Text { get; set; }

        public Word(string color, string text)
        {
            Color = color;
            Text = text;
        }
        public override string ToString()
        {
            return $"Texte : {Text}\nCouleur :({Color})";
        }
    }
}
