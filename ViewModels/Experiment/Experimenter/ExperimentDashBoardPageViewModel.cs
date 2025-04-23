using System.ComponentModel;
using System.Runtime.CompilerServices;
using StroopApp.Views.Experiment.Experimenter;
using StroopApp.Models;

namespace StroopApp.ViewModels.Experiment
{
    public class ExperimentDashBoardPageViewModel : INotifyPropertyChanged
    {
        readonly SharedExperimentData _experimentContext;
        public ExperimentDashBoardPageViewModel( ExperimentSettings settings)
        {
            _experimentContext = settings.ExperimentContext;
            _experimentContext.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_experimentContext.IsExperimentFinished)
                    && _experimentContext.IsExperimentFinished)
                {
                    App.ExperimentWindowNavigationService.NavigateTo(
                        () => new EndExperimentPage(settings));
                }
            };
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
