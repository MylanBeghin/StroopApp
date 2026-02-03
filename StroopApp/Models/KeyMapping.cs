using StroopApp.Core;
using System.Windows.Input;

namespace StroopApp.Models
{
    /// <summary>
    /// Represents a key binding associated with a specific color used in the experiment.
    /// </summary>

    public class KeyMapping : ModelBase
    {
        private string _color;
        public string Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }
        private Key _key;
        public Key Key
        {
            get => _key;
            set
            {
                if (_key != value)
                {
                    _key = value;
                    OnPropertyChanged();
                }
            }
        }
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
