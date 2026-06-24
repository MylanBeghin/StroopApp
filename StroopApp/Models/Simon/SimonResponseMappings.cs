using System.Windows.Input;

namespace StroopApp.Models.Simon
{
    public class SimonResponseMappings
    {
        public SimonResponseMapping Left { get; set; }
        public SimonResponseMapping Right { get; set; }
        public SimonResponseMappings()
        {
            Left = new(SimonAnswer.Left, Key.Left, "#0000FF");
            Right = new(SimonAnswer.Right, Key.Right, "#FF0000");
        }
    }
}
