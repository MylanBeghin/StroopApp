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
            Red = new KeyMapping("Rouge", Key.R);
            Blue = new KeyMapping("Bleu", Key.B);
            Green = new KeyMapping("Vert", Key.V);
            Yellow = new KeyMapping("Jaune", Key.J);
        }
    }
}
