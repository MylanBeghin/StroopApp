using StroopApp.Core;
using StroopApp.Services.Navigation;

namespace StroopApp.ViewModels.Configuration
{
    public class SimonConfigurationPageViewModel : ViewModelBase
    {
        private readonly INavigationService _experimenterNavigationService;

        public SimonConfigurationPageViewModel(INavigationService experimenterNavigationService)
        {
            _experimenterNavigationService = experimenterNavigationService;
        }
    }
}
