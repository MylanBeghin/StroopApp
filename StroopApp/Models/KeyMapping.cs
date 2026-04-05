using System.Windows.Input;

namespace StroopApp.Models
{
    /// <summary>
    /// Represents a key binding associated with a specific color used in the experiment.
    /// </summary>

    public class KeyMapping
    {
        public string Color { get; set; }
        public Key Key { get; set; }
        /// <summary>
        /// Initializes a new key mapping for the specified color and key.
        /// </summary>
        public KeyMapping(string color, Key key)
        {
            Color = color;
            Key = key;
        }
    }
}
