using System.Windows.Media;

using StroopApp.Core;

namespace StroopApp.ViewModels.Experiment.Participant.Stroop
{
    /// <summary>
    /// ViewModel for displaying Stroop stimulus words with color mapping.
    /// Supports bilingual color names (English/French) for localization.
    /// </summary>
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
				OnPropertyChanged(nameof(ForegroundBrush));
			}
		}

		public Brush ForegroundBrush => _color switch
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
