using System.Windows.Input;

namespace StroopApp.Models.Simon
{
    public class SimonKeyMappings
    {
        public SimonKeyMapping Left { get; set; }
        public SimonKeyMapping Right { get; set; }
        public SimonKeyMappings()
        {
            Left = new(SimonAnswer.Left, Key.Left);
            Right = new(SimonAnswer.Right, Key.Right);
        }
    }
}
