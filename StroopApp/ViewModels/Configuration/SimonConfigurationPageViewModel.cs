using StroopApp.Core;
using StroopApp.Services.Navigation;
using StroopApp.ViewModels.Configuration.ProtocolConfiguration;

namespace StroopApp.ViewModels.Configuration
{
    public class SimonConfigurationPageViewModel : ViewModelBase
    {
        private SimonProtocolConfigurationViewModel _protocolConfigurationViewModel;
        private readonly INavigationService _experimenterNavigationService;

        public SimonProtocolConfigurationViewModel ProtocolConfigurationViewModel
        {
            get { return _protocolConfigurationViewModel; }
            set { _protocolConfigurationViewModel = value; }
        }

        public SimonConfigurationPageViewModel(INavigationService experimenterNavigationService)
        {
            _protocolConfigurationViewModel = new SimonProtocolConfigurationViewModel();
            _experimenterNavigationService = experimenterNavigationService;
        }
    }
}
