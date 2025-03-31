using StroopApp.Services.Navigation;
using StroopApp.Models;
using System.Runtime.CompilerServices;
using System.ComponentModel;
namespace StroopApp.ViewModels.Experiment
{
    public class ExperimentDashBoardPageViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService NavigationService;
        public ExperimentSettings _settings { get; set; }
        public ExperimentDashBoardPageViewModel(INavigationService navigationService, ExperimentSettings settings)
        {
            NavigationService = navigationService;
            _settings = settings;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
