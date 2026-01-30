using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.Services.Language;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.Views.Experiment.Experimenter;
using System.ComponentModel;
using System.Windows.Input;

namespace StroopApp.ViewModels.Experiment.Experimenter
{
    /// <summary>
    /// ViewModel for the experiment dashboard, monitoring block completion and handling manual task termination.
    /// Automatically navigates to end page when block finishes.
    /// </summary>
    public class ExperimentDashBoardPageViewModel : ViewModelBase, IDisposable
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

            _experimentContext.PropertyChanged += OnExperimentContextPropertyChanged;

            StopTaskCommand = new RelayCommand(async _ => await StopTaskAsync());
        }
        private void OnExperimentContextPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_experimentContext.IsBlockFinished)
                && _experimentContext.IsBlockFinished)
            {
                _experimenterNavigationService.NavigateTo(
                    () => new EndExperimentPage(_settings, _experimenterNavigationService, _windowManager, _languageService));
            }
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
        public void Dispose()
        {
            _experimentContext.PropertyChanged -= OnExperimentContextPropertyChanged;
        }
    }
}
