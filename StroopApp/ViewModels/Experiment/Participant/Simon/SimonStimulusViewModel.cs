using CommunityToolkit.Mvvm.ComponentModel;
using StroopApp.Core;
using StroopApp.Models.Simon;
using System.Windows.Media;

namespace StroopApp.ViewModels.Experiment.Participant.Simon
{
    public partial class SimonStimulusViewModel : ViewModelBase
    {
        [ObservableProperty]
        private StimulusPosition _position;

        [ObservableProperty]
        private string _color = "";

        partial void OnColorChanged(string value)
        {
            OnPropertyChanged(nameof(ForegroundBrush));
        }

        public Brush ForegroundBrush
        {
            get
            {
                try
                {
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString(Color));
                }
                catch
                {
                    return Brushes.White;
                }
            }
        }

        public SimonStimulusViewModel(StimulusPosition position, string color)
        {
            Position = position;
            Color = color;
        }
    }
}
