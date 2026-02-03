using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.Services.Charts;
using StroopApp.Services.Exportation;
using StroopApp.Services.Language;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.Views;
using StroopApp.Views.Experiment.Experimenter.End;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace StroopApp.ViewModels.Experiment.Experimenter.End
{
    /// <summary>
    /// ViewModel for the end-of-experiment page, handling continuation to next block, 
    /// starting new experiments, data export, and application shutdown with confirmations.
    /// </summary>
    public class EndExperimentPageViewModel : ViewModelBase
    {
        public ExperimentSettings Settings { get; }
        public ObservableCollection<Block> Blocks { get; }

        private readonly IExportationService _exportationService;
        private readonly INavigationService _experimenterNavigationService;
        private readonly IWindowManager _windowManager;
        private readonly ILanguageService _languageService;
        private readonly ExperimentChartFactory _chartFactory;

        public ICommand ContinueCommand { get; }
        public ICommand NewExperimentCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand QuitWithoutExportCommand { get; }
        public ICommand QuitWithExportCommand { get; }

        private string _currentParticipant;
        public string CurrentParticipant
        {
            get => _currentParticipant;
            set
            {
                _currentParticipant = value;
                OnPropertyChanged();
            }
        }

        private string _currentProfile;
        public string CurrentProfile
        {
            get => _currentProfile;
            set
            {
                _currentProfile = value;
                OnPropertyChanged();
            }
        }

        public EndExperimentPageViewModel(ExperimentSettings settings,
                                  IExportationService exportationService,
                                  INavigationService experimenterNavigationService,
                                  IWindowManager windowManager,
                                  ILanguageService languageService)
        {
            Settings = settings;
            _exportationService = exportationService;
            _experimenterNavigationService = experimenterNavigationService;
            _windowManager = windowManager;
            _languageService = languageService;
            _chartFactory = new ExperimentChartFactory();

            ContinueCommand = new RelayCommand(_ => Continue());
            NewExperimentCommand = new RelayCommand(async _ => await NewExperimentAsync());
            ExportCommand = new RelayCommand(async _ => await ExportAsync());
            QuitWithoutExportCommand = new RelayCommand(async _ => await QuitWithoutExportAsync());
            QuitWithExportCommand = new RelayCommand(async _ => await QuitWithExportAsync());

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

                _experimenterNavigationService.NavigateTo(() =>
                    new ConfigurationPage(Settings, _experimenterNavigationService, _windowManager, _languageService));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Continue: {ex.Message}");
            }
        }

        private async Task NewExperimentAsync()
        {
            try
            {
                bool confirmed = await ShowConfirmationDialogAsync(Strings.Title_ConfirmNewExperiment, Strings.Message_ConfirmNewExperiment);
                if (confirmed)
                {
                    Settings.Reset();
                    _windowManager.CloseParticipantWindow();
                    _experimenterNavigationService.NavigateTo(() =>
                        new ConfigurationPage(Settings, _experimenterNavigationService, _windowManager, _languageService));
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }

        private async Task ExportAsync()
        {
            try
            {
                var exportEndExperimentWindow = new ExportEndExperimentWindow(Settings, _exportationService, _experimenterNavigationService, _windowManager, _languageService);
                exportEndExperimentWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }

        private async Task QuitWithoutExportAsync()
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

        private async Task QuitWithExportAsync()
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
