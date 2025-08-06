using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using StroopApp.Core;

namespace StroopApp.Models
{
	/// <summary>
	/// Represents a key binding associated with a specific color used in the experiment.
	/// </summary>

	public class KeyMapping : ModelBase
	{
		private Key _key;
		public string Color { get; set; }
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

		public KeyMapping(string color, Key key)
		{
			Color = color;
			Key = key;
		}
	}
}
