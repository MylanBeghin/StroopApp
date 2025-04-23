using StroopApp.Models;
using StroopApp.Services.Navigation;

namespace StroopApp.ViewModels.Experiment.Participant
{
    public class ParticipantWindowViewModel
    {
        public ExperimentSettings Settings { get; }
        public INavigationService NavigationService { get; }
        public ParticipantWindowViewModel(ExperimentSettings settings, INavigationService navigationService)
        {
            Settings = settings;
            NavigationService = navigationService;
        }
    }
}
