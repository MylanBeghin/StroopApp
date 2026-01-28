using System.Threading;
using System.Windows.Input;
using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.Services.Language;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.Views.Experiment.Experimenter;

namespace StroopApp.ViewModels.Experiment.Experimenter
{
	public class ExperimentDashBoardPageViewModel : ViewModelBase
	{
		private readonly ExperimentSettings _settings;
		private readonly INavigationService _experimenterNavigationService;
		private readonly SharedExperimentData _experimentContext;
		private readonly IWindowManager _windowManager;
		private readonly ILanguageService _languageService;

		public ICommand StopTaskCommand { get; }

		public ExperimentDashBoardPageViewModel(ExperimentSettings settings, 
			INavigationService experimenterNavigationService, 
			IWindowManager windowManager, 
			ILanguageService languageService)
		{
			_settings = settings;
			_experimentContext = settings.ExperimentContext;
			_experimenterNavigationService = experimenterNavigationService;
			_windowManager = windowManager;
			_languageService = languageService;

			_experimentContext.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(_experimentContext.IsBlockFinished)
					&& _experimentContext.IsBlockFinished)
				{
					_experimenterNavigationService.NavigateTo(
						() => new EndExperimentPage(_settings, _experimenterNavigationService, _windowManager, _languageService));
				}
			};
			
			StopTaskCommand = new RelayCommand(async _ => await StopTaskAsync());
		}

		public async Task StopTaskAsync()
		{
			try
			{
				bool confirmed = await ShowConfirmationDialogAsync(Strings.Title_ConfirmStopTask, Strings.Message_StopTask);
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
			catch (Exception ex)
			{
				await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
			}
		}
	}
}
