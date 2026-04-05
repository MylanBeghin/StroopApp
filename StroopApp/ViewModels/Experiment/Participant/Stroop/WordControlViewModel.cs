using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

using StroopApp.Core;

namespace StroopApp.ViewModels.Experiment.Participant.Stroop
{
	/// <summary>
	/// ViewModel for displaying Stroop stimulus words with color mapping.
	/// Supports bilingual color names (English/French) for localization.
	/// </summary>
	public partial class WordControlViewModel : ViewModelBase
	{
	  [ObservableProperty]
		private string _label;

	  [ObservableProperty]
		private string _color;

		partial void OnColorChanged(string value)
		{
			OnPropertyChanged(nameof(ForegroundBrush));
		}

		public Brush ForegroundBrush => Color switch
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
