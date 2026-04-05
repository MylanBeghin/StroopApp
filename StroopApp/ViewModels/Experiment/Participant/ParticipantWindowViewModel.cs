using StroopApp.Services.Navigation;
using StroopApp.ViewModels.State;
using StroopApp.Views.Experiment.Participant;
using System.ComponentModel;

namespace StroopApp.ViewModels.Experiment.Participant
{
    public class ParticipantWindowViewModel : IDisposable
    {
        private readonly ExperimentSettingsViewModel _settings;
        private readonly INavigationService _participantWindowNavigationService;

        public ParticipantWindowViewModel(ExperimentSettingsViewModel settings, INavigationService participantWindowNavigationService)
        {
            _settings = settings;
            _participantWindowNavigationService = participantWindowNavigationService;

            _settings.ExperimentContext.PropertyChanged += ExperimentContext_PropertyChanged;

            _participantWindowNavigationService.NavigateTo(() => new InstructionsPage(_settings, _participantWindowNavigationService));
        }

        private void ExperimentContext_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_settings.ExperimentContext.IsBlockFinished)
                && _settings.ExperimentContext.IsBlockFinished)
            {
                _participantWindowNavigationService.NavigateTo(() => new EndInstructionsPage());
            }
        }

        public void Dispose()
        {
            _settings.ExperimentContext.PropertyChanged -= ExperimentContext_PropertyChanged;
        }
    }
}