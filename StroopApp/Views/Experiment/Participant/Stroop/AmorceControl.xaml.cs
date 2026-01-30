using StroopApp.Models;
using System.Windows.Controls;
namespace StroopApp.Views.Experiment.Participant.Stroop
{
    public partial class AmorceControl : UserControl
    {
        public AmorceControl(VisualCueType amorce)
        {
            InitializeComponent();
            DataContext = new VisualCueControlViewModel(amorce);
        }
    }
}
