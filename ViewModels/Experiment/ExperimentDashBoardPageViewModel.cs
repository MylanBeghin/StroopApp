using StroopApp.Services.Navigation;

namespace StroopApp.ViewModels.Experiment
{
    public class ExperiementDashBoardPageViewModel
    {
        private readonly INavigationService NavigationService;
        
        public ExperiementDashBoardPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}
