using System.Windows.Input;

namespace StroopApp.Models.Simon
{
    public class SimonResponseMapping
    {
        public SimonAnswer Answer { get; set; }
        public Key Key { get; set; }
        public string Color { get; set; }
        public SimonResponseMapping (SimonAnswer answer, Key key, string color){
            Answer = answer;
            Key = key;
            Color = color;
        } 
        
    }
}


