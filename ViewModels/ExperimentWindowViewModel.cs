using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.Views;

namespace StroopApp.ViewModels
{
    public class ExperimentWindowViewModel
    {
        public ExperimentWindowViewModel(ExperimentSettings settings, INavigationService experimentNavigationService, IWindowManager windowManager)
        {
            experimentNavigationService.NavigateTo(() => new ConfigurationPage(settings, experimentNavigationService, windowManager));
        }
        
    }
}
