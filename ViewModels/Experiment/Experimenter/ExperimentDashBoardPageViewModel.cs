using System.ComponentModel;
using System.Runtime.CompilerServices;
using StroopApp.Views.Experiment.Experimenter;
using StroopApp.Models;
using StroopApp.Core;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;

namespace StroopApp.ViewModels.Experiment
{
    public class ExperimentDashBoardPageViewModel : ViewModelBase
    {
        readonly SharedExperimentData _experimentContext;
        public ExperimentDashBoardPageViewModel( ExperimentSettings settings, INavigationService experimenterNavigationService, IWindowManager windowManager)
        {
            _experimentContext = settings.ExperimentContext;
            _experimentContext.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_experimentContext.IsBlockFinished)
                    && _experimentContext.IsBlockFinished)
                {
                    experimenterNavigationService.NavigateTo(
                        () => new EndExperimentPage(settings, experimenterNavigationService, windowManager));
                }
            };
        }
    }
}
