using DocumentFormat.OpenXml.Wordprocessing;
using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Views.Experiment.Participant;

namespace StroopApp.ViewModels.Experiment.Participant
{
    public class ParticipantWindowViewModel
    {
        public ParticipantWindowViewModel(ExperimentSettings settings, INavigationService participantWindowNavigationService)
        {
            participantWindowNavigationService.NavigateTo(() => new InstructionsPage(settings, participantWindowNavigationService));
        }
    }
}
