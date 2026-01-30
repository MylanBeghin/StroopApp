using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.Services.Exportation;
using StroopApp.Services.Language;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.Views;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace StroopApp.ViewModels.Experiment.Experimenter.End
{
    /// <summary>
    /// ViewModel for the export dialog window, managing export workflow states (exporting, success, error),
    /// file opening, and post-export actions (new experiment, quit).
    /// </summary>
    public class ExportEndExperimentWindowViewModel : ViewModelBase
    {
        private readonly ExperimentSettings _settings;
        private readonly IExportationService _exportationService;
        private readonly Window _parentWindow;
        private readonly INavigationService _navigationService;
        private readonly IWindowManager _windowManager;
        private readonly ILanguageService _languageService;

        private bool _isExporting;
        public bool IsExporting
        {
            get => _isExporting;
            set { _isExporting = value; OnPropertyChanged(); OnPropertyChanged(nameof(DefaultView)); }
        }

        private bool _exportSuccess;
        public bool ExportSuccess
        {
            get => _exportSuccess;
            set { _exportSuccess = value; OnPropertyChanged(); OnPropertyChanged(nameof(DefaultView)); }
        }

        private bool _exportError;
        public bool ExportError
        {
            get => _exportError;
            set { _exportError = value; OnPropertyChanged(); OnPropertyChanged(nameof(DefaultView)); }
        }

        public bool DefaultView => !IsExporting && !ExportSuccess && !ExportError;

        private string _exportPath;
        public string ExportPath
        {
            get => _exportPath;
            set
            {
                if (_exportPath != value)
                {
                    _exportPath = value;
                    OnPropertyChanged(nameof(ExportPath));
                    ResetState();
                }
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand ExportCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand RetryCommand { get; }
        public ICommand NewExperimentCommand { get; }
        public ICommand QuitCommand { get; }
        public ICommand OpenAndSelectTodayExportFileCommand { get; }
        public ICommand ReExportCommand { get; }

        public ExportEndExperimentWindowViewModel(ExperimentSettings settings, IExportationService exportationService, Window parentWindow, INavigationService navigationService, IWindowManager windowManager, ILanguageService languageService)
        {
            _settings = settings;
            _exportationService = exportationService;
            _parentWindow = parentWindow;
            _navigationService = navigationService;
            _windowManager = windowManager;
            _languageService = languageService;

            ExportPath = _exportationService.LoadExportFolderPath();
            ExportCommand = new CommunityToolkit.Mvvm.Input.AsyncRelayCommand(ExportAsync, CanExport);
            CancelCommand = new RelayCommand(_ => CancelExport());
            RetryCommand = new CommunityToolkit.Mvvm.Input.AsyncRelayCommand(ExportAsync, CanExport);
            OpenAndSelectTodayExportFileCommand = new RelayCommand(async _ => await OpenAndSelectTodayExportFileAsync());
            NewExperimentCommand = new RelayCommand(async _ => await NewExperimentAsync());
            QuitCommand = new RelayCommand(async _ => await QuitAsync());
            ReExportCommand = new RelayCommand(_ => ReExport());

            _settings.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_settings.ExportFolderPath))
                {
                    OnPropertyChanged(nameof(ExportPath));
                    ExportPath = _exportationService.LoadExportFolderPath();
                    ResetState();
                }
            };

            ResetState();
        }

        private void ResetState()
        {
            IsExporting = false;
            ExportSuccess = false;
            ExportError = false;
            ErrorMessage = string.Empty;
        }
        private void CancelExport()
        {
            IsExporting = false;
            _parentWindow.Close();

        }
        private void ReExport()
        {
            ResetState();
        }

        private bool CanExport() => !string.IsNullOrWhiteSpace(ExportPath) && !IsExporting;

        private async Task ExportAsync()
        {
            ResetState();
            IsExporting = true;
            try
            {
                _exportationService.LoadExportFolderPath();
                await _exportationService.ExportDataAsync();
                IsExporting = false;
                ExportSuccess = true;
                _settings.ExperimentContext.HasUnsavedExports = false;
            }
            catch (Exception ex)
            {
                IsExporting = false;
                ExportError = true;
                ErrorMessage = $"{Strings.Error_ExportFailed}\n{ex.Message}"; ;
            }
        }
        private async Task OpenAndSelectTodayExportFileAsync()
        {
            try
            {
                ExportPath = _exportationService.LoadExportFolderPath();

                string today = DateTime.Now.ToString("yyyy-MM-dd");
                string participantId = _settings.Participant.Id.ToString();
                string folderPath = Path.Combine(ExportPath, "Results", participantId, today);

                var files = Directory.Exists(folderPath) ? Directory.GetFiles(folderPath, "*.xlsx") : Array.Empty<string>();
                string filePath = files.OrderByDescending(File.GetCreationTime).FirstOrDefault();

                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    Process.Start("explorer.exe", $"/select,\"{filePath}\"");
                }
                else if (Directory.Exists(folderPath))
                {
                    Process.Start("explorer.exe", $"\"{folderPath}\"");
                }
                else
                {
                    await ShowErrorDialogAsync(Strings.Error_FileOrFolderNotFound);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync(string.Format(Strings.Error_OpeningFile, ex.Message));
            }
        }

        private async Task NewExperimentAsync()
        {
            try
            {
                _settings.Reset();
                _parentWindow.DialogResult = true;
                _parentWindow.Close();
                _navigationService.NavigateTo(() => new ConfigurationPage(_settings, _navigationService, _windowManager, _languageService));
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync(string.Format(Strings.Error_CreatingNewExperiment, ex.Message));
            }
        }

        private async Task QuitAsync()
        {
            try
            {
                if (_settings.ExperimentContext.HasUnsavedExports)
                {
                    if (await ShowConfirmationDialogAsync(
                        Strings.Title_ConfirmShutDown,
                        Strings.Message_ConfirmExitWithoutExport))
                    {
                        Application.Current.Shutdown();
                    }
                }
                else
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
