using System.Windows.Media;

using StroopApp.Core;

namespace StroopApp.ViewModels.Experiment.Participant.Stroop
{


	public class WordControlViewModel : ViewModelBase
	{
		private string label;
		public string Label
		{
			get => label;
			set
			{
				label = value;
				OnPropertyChanged();
			}
		}

		private string color;
		public string Color
		{
			get => color;
			set
			{
				color = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(ForegroundBrush));
			}
		}

		public Brush ForegroundBrush => color switch
		{
			"Red" or "Rouge" => Brushes.Red,
			"Blue" or "Bleu" => Brushes.Blue,
			"Green" or "Vert" => Brushes.Green,
			"Yellow" or "Jaune" => Brushes.Yellow,
			_ => Brushes.Black
		};

		public WordControlViewModel(string label, string color)
		{
			Label = label;
			Color = color;
		}
	}

}
