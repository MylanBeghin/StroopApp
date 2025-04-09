using System.ComponentModel;
using System.Runtime.CompilerServices;
using StroopApp.Models;
using StroopApp.Services.Navigation;

namespace StroopApp.ViewModels.Experiment
{
    public class ExperimentDashBoardPageViewModel : INotifyPropertyChanged
    {
        public ExperimentDashBoardPageViewModel(INavigationService navigationService, ExperimentSettings settings)
        {

        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
