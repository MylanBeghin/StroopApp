using Ookii.Dialogs.Wpf;
using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services.Exportation;
using System.Windows.Input;

namespace StroopApp.ViewModels.Configuration
{
    public class ExportFolderSelectorViewModel : ViewModelBase
    {
        private readonly IExportationService _exportationService;
        public ExperimentSettings Settings
        {
            get;
        }

        public ICommand BrowseCommand
        {
            get;
        }

        public ExportFolderSelectorViewModel(ExperimentSettings settings, IExportationService exportationService)
        {
            Settings = settings;
            _exportationService = exportationService;
            Settings.ExportFolderPath = _exportationService.LoadExportFolderPath();

            BrowseCommand = new RelayCommand(() =>
            {
                var dlg = new VistaFolderBrowserDialog
                {
                    Description = "Sélectionner le dossier d’exportation pour vos résultats",
                    SelectedPath = Settings.ExportFolderPath
                };
                if (dlg.ShowDialog() == true)
                {
                    Settings.ExportFolderPath = dlg.SelectedPath;
                    _exportationService.SaveExportFolderPath(dlg.SelectedPath);
                }
            });
        }
    }
}
