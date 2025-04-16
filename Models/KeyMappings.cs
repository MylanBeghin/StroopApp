using System.Windows.Input;

namespace StroopApp.Models
{
    public class KeyMappings
    {
        public KeyMapping Red { get; set; }
        public KeyMapping Blue { get; set; }
        public KeyMapping Green { get; set; }
        public KeyMapping Yellow { get; set; }

        public KeyMappings()
        {
            Red = new KeyMapping("Red", Key.R);
            Blue = new KeyMapping("Blue", Key.B);
            Green = new KeyMapping("Green", Key.V);
            Yellow = new KeyMapping("Yellow", Key.J);
        }
    }
}
