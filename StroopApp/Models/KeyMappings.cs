using System.Windows.Input;

namespace StroopApp.Models
{
	/// <summary>
	/// Stores the set of key mappings for each color used in the Stroop task (Red, Blue, Green, Yellow).
	/// </summary>
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
