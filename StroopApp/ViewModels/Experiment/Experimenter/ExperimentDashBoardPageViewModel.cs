using System.Threading;
using System.Windows.Input;
using System.Windows.Navigation;

using DocumentFormat.OpenXml.Wordprocessing;

using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.Views.Experiment.Experimenter;
using StroopApp.Views.Experiment.Participant;

namespace StroopApp.ViewModels.Experiment
{
	public class ExperimentDashBoardPageViewModel : ViewModelBase
	{
		readonly ExperimentSettings _settings;
		readonly INavigationService _experimenterNavigationService;
		readonly SharedExperimentData _experimentContext;
		readonly IWindowManager _windowManager;

		public ICommand StopTaskCommand
		{
			get;
		}
		public ExperimentDashBoardPageViewModel(ExperimentSettings settings, INavigationService experimenterNavigationService, IWindowManager windowManager)
		{
			_settings = settings;
			_experimentContext = settings.ExperimentContext;
			_experimenterNavigationService = experimenterNavigationService;
			_windowManager = windowManager;
			_experimentContext.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(_experimentContext.IsBlockFinished)
					&& _experimentContext.IsBlockFinished)
				{
					_experimenterNavigationService.NavigateTo(
						() => new EndExperimentPage(_settings, _experimenterNavigationService, _windowManager));
				}
			};
			StopTaskCommand = new RelayCommand(async _ => await StopTaskAsync());
		}
		async public Task StopTaskAsync()
		{
			bool confirmed = await ShowConfirmationDialog(Strings.Title_ConfirmStopTask, Strings.Message_StopTask);
			if (confirmed)
			{
				_settings.ExperimentContext.IsTaskStopped = true;
				if (_experimentContext.CurrentBlock != null)
				{
					_experimentContext.CurrentBlock.CalculateValues();
					_experimentContext.CurrentTrial = null;

				}
				_experimentContext.IsBlockFinished = true;
			}
		}
	}
}
