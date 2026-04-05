using StroopApp.Models;
using System.Windows.Controls;
namespace StroopApp.Views.Experiment.Participant.Stroop
{
    public partial class VisualCueControl : UserControl
    {
        public VisualCueControl(VisualCueType visualCue)
        {
            InitializeComponent();
            DataContext = new VisualCueControlViewModel(visualCue);
        }
    }
}
