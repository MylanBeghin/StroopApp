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
using StroopApp.Views;
using StroopApp.Views.Experiment.Experimenter.End;
using System.Collections.ObjectModel;
using System.Windows;

namespace StroopApp.ViewModels.Experiment.Experimenter.End
{
    /// <summary>
    /// ViewModel for the end-of-experiment page, handling continuation to next block, 
    /// starting new experiments, data export, and application shutdown with confirmations.
    /// </summary>
    public partial class EndExperimentPageViewModel : ViewModelBase
    {
        public ExperimentSettingsViewModel Settings { get; }
        public ObservableCollection<Block> Blocks { get; }

        private readonly IExportationService _exportationService;
        private readonly INavigationService _experimenterNavigationService;
        private readonly IWindowManager _windowManager;
        private readonly ExperimentChartFactory _chartFactory;

        [ObservableProperty]
        private string _currentParticipant = string.Empty;

        [ObservableProperty]
        private string _currentProfile = string.Empty;

        public EndExperimentPageViewModel(ExperimentSettingsViewModel settings,
                                  IExportationService exportationService,
                                  INavigationService experimenterNavigationService,
                                  IWindowManager windowManager)
        {
            Settings = settings;
            _exportationService = exportationService;
            _experimenterNavigationService = experimenterNavigationService;
            _windowManager = windowManager;
            _chartFactory = new ExperimentChartFactory();

            Blocks = Settings.ExperimentContext.Blocks;
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
        private void Continue()
        {
            try
            {
                Settings.ExperimentContext.ReactionPoints.Clear();
                Settings.ExperimentContext.NewColumnSerie();
                Settings.Block++;
                Settings.ExperimentContext.IsBlockFinished = false;
                Settings.ExperimentContext.IsParticipantSelectionEnabled = false;
                Settings.ExperimentContext.HasUnsavedExports = true;

                _experimenterNavigationService.NavigateTo<ConfigurationPage>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Continue: {ex.Message}");
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
                    Settings.Reset();
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
    }
}
