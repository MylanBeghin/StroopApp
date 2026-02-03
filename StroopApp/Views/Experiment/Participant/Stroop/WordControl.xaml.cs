using StroopApp.ViewModels.Experiment.Participant.Stroop;
using System.Windows.Controls;

namespace StroopApp.Views.Experiment.Participant.Stroop
{
    public partial class WordControl : UserControl
    {
        public WordControl(string label, string color)
        {
            InitializeComponent();
            DataContext = new WordControlViewModel(label, color);
        }
    }
}
