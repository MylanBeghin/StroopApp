using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.Services.Charts;
using StroopApp.Services.Exportation;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.ViewModels.State;
using StroopApp.ViewModels.Experiment.Experimenter;
using StroopApp.Views;
using StroopApp.Views.Experiment.Experimenter.End;
using System.Collections.ObjectModel;
using System.Windows;
using StroopApp.Views.Configuration;
using StroopApp.Services.Session;

namespace StroopApp.ViewModels.Experiment.Experimenter.End
{
    /// <summary>
    /// ViewModel for the end-of-experiment page, handling continuation to next block, 
    /// starting new experiments, data export, and application shutdown with confirmations.
    /// </summary>
    public partial class EndExperimentPageViewModel : ViewModelBase, IDisposable
    {
        public ExperimentSettingsViewModel Settings { get; }
        public ObservableCollection<Block> Blocks { get; }
        public GlobalGraphViewModel GlobalGraphViewModel { get; }
        public LiveReactionTimeViewModel LiveReactionTimeViewModel { get; }

        private readonly IExportationService _exportationService;
        private readonly INavigationService _experimenterNavigationService;
        private readonly IWindowManager _windowManager;
        private readonly ExperimentChartFactory _chartFactory;
        private readonly IExperimentSessionService _sessionService;

        [ObservableProperty]
        private string _currentParticipant = string.Empty;

        [ObservableProperty]
        private string _currentProfile = string.Empty;

        public EndExperimentPageViewModel(ExperimentSettingsViewModel settings,
                                  IExportationService exportationService,
                                  INavigationService experimenterNavigationService,
                                  IWindowManager windowManager,
                                  IExperimentSessionService sessionService)
        {
            Settings = settings;
            _exportationService = exportationService;
            _experimenterNavigationService = experimenterNavigationService;
            _windowManager = windowManager;
            _chartFactory = new ExperimentChartFactory();
            _sessionService = sessionService;

            Blocks = Settings.ExperimentContext.Blocks;
            GlobalGraphViewModel = new GlobalGraphViewModel(settings);
            LiveReactionTimeViewModel = new LiveReactionTimeViewModel(settings);
            CurrentParticipant = string.Format(Strings.Label_CurrentParticipant, Settings.Participant.Id);
            CurrentProfile = string.Format(Strings.Label_CurrentProfile, Settings.CurrentProfile.ProfileName);

            UpdateBlock();
        }

        private void UpdateBlock()
        {
            var pointsSnapshot = new ObservableCollection<ReactionTimePoint>(Settings.ExperimentContext.ReactionPoints);
            Settings.ExperimentContext.ColumnSerie = _chartFactory.CreateSnapshotColumnSerie(pointsSnapshot);
        }

        [RelayCommand]
        private async Task Continue()
        {
            try
            {
                _sessionService.PrepareNextBlock();
                if(Settings.CurrentProfile.TaskType==TaskType.Stroop)
                    _experimenterNavigationService.NavigateTo<ConfigurationPage>();
                else if (Settings.CurrentProfile.TaskType == TaskType.Simon)
                    _experimenterNavigationService.NavigateTo<SimonConfigurationPage>();
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task NewExperiment()
        {
            try
            {
                bool confirmed = await ShowConfirmationDialogAsync(Strings.Title_ConfirmNewExperiment, Strings.Message_ConfirmNewExperiment);
                if (confirmed)
                {
                    _sessionService.ResetForNewExperiment();
                    _windowManager.CloseParticipantWindow();
                    _experimenterNavigationService.NavigateTo<ConfigurationPage>();
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task Export()
        {
            try
            {
                var exportEndExperimentWindow = new ExportEndExperimentWindow(Settings, _exportationService, _experimenterNavigationService);
                exportEndExperimentWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task QuitWithoutExport()
        {
            try
            {
                if (await ShowConfirmationDialogAsync(Strings.Title_ConfirmShutDown, Strings.Message_ConfirmExitWithoutExport))
                {
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task QuitWithExport()
        {
            try
            {
                if (await ShowConfirmationDialogAsync(Strings.Title_ConfirmShutDown, Strings.Message_ConfirmExitWithExport))
                {
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }
        public void Dispose()
        {
            LiveReactionTimeViewModel.Dispose();
        }
    }
}
