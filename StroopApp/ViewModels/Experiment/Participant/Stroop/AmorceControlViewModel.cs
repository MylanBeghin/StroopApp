using StroopApp.Models;

namespace StroopApp.ViewModels.Experiment.Participant.Stroop
{
    public class AmorceControlViewModel
    {
        public AmorceType Amorce { get; set; }
        public AmorceControlViewModel(AmorceType amorce)
        {
            Amorce = amorce;
        }
    }
}
