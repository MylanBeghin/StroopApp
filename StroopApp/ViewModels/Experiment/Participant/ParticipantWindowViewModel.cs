using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Views.Experiment.Participant;

namespace StroopApp.ViewModels.Experiment.Participant
{
	public class ParticipantWindowViewModel
	{
		private readonly ExperimentSettings _settings;
		private readonly INavigationService _participantWindowNavigationService;

		public ParticipantWindowViewModel(ExperimentSettings settings, INavigationService participantWindowNavigationService)
		{
			_settings = settings;
			_participantWindowNavigationService = participantWindowNavigationService;


			_settings.ExperimentContext.PropertyChanged += ExperimentContext_PropertyChanged;


			participantWindowNavigationService.NavigateTo(() => new InstructionsPage(settings, participantWindowNavigationService));
		}

		private void ExperimentContext_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{

			if (e.PropertyName == nameof(_settings.ExperimentContext.IsBlockFinished)
				&& _settings.ExperimentContext.IsBlockFinished)
			{
				_participantWindowNavigationService.NavigateTo(() => new EndInstructionsPage());
			}
		}
		public void Dispose()
		{
			if (_settings?.ExperimentContext != null)
			{
				_settings.ExperimentContext.PropertyChanged -= ExperimentContext_PropertyChanged;
			}
		}
	}
}
