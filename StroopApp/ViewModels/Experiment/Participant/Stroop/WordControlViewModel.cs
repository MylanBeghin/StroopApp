using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using StroopApp.Core;

namespace StroopApp.ViewModels.Experiment.Participant.Stroop
{
	public class WordControlViewModel : ViewModelBase
	{
		private string _label;
		public string Label
		{
			get => _label;
			set
			{
				_label = value;
				OnPropertyChanged();
			}
		}
		private string _color;
		public string Color
		{
			get => _color;
			set
			{
				_color = value;
				OnPropertyChanged();
			}
		}

		public WordControlViewModel(string label, string color)
		{
			switch (label)
			{
				case "Blue":
				Label = "Bleu";
				break;
				case "Red":
				Label = "Rouge";
				break;
				case "Green":
				Label = "Vert";
				break;
				case "Yellow":
				Label = "Jaune";
				break;
			}
			Color = color;
		}
	}
}
