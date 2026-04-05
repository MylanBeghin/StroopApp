using CommunityToolkit.Mvvm.Input;
using Ookii.Dialogs.Wpf;
using StroopApp.Core;
using StroopApp.Resources;
using StroopApp.Services.Exportation;
using StroopApp.ViewModels.State;

namespace StroopApp.ViewModels.Configuration
{
    /// <summary>
    /// ViewModel for selecting and managing the experiment data export folder path.
    /// Provides folder browsing dialog and persists the selected path via IExportationService.
    /// </summary>
    public partial class ExportFolderSelectorViewModel : ViewModelBase
    {
        private readonly IExportationService _exportationService;
        public ExperimentSettingsViewModel Settings { get; }

        public ExportFolderSelectorViewModel(ExperimentSettingsViewModel settings, IExportationService exportationService)
        {
            Settings = settings;
            _exportationService = exportationService;
            Settings.ExportFolderPath = _exportationService.LoadExportFolderPath();
        }

        [RelayCommand]
        private void Browse()
        {
            var dlg = new VistaFolderBrowserDialog
            {
                Description = Strings.Description_ExportFolderDialog,
                SelectedPath = Settings.ExportFolderPath
            };
            if (dlg.ShowDialog() == true)
            {
                Settings.ExportFolderPath = dlg.SelectedPath;
                _exportationService.SaveExportFolderPath(dlg.SelectedPath);
            }
        }
    }
}
