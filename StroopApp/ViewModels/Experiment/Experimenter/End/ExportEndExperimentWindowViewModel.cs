using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using DocumentFormat.OpenXml.Wordprocessing;

using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services.Exportation;

namespace StroopApp.ViewModels.Experiment.Experimenter.End
{
	internal class ExportEndExperimentWindowViewModel : ViewModelBase
	{
		private readonly ExperimentSettings _settings;
		private readonly IExportationService _exportationService;
		private readonly Window _parentWindow;

		public ICommand ExportCommand { get; }
		public ICommand BrowseFolderCommand { get; }
		public ICommand RetryCommand { get; }
		public ICommand NewExperimentCommand { get; }
		public ICommand QuitCommand { get; }

		public ExportEndExperimentWindowViewModel(ExperimentSettings settings, IExportationService exportationService, Window parentWindow)
		{
			_settings = settings;
			_exportationService = exportationService;
			_parentWindow = parentWindow;

			ExportPath = _exportationService.LoadExportFolderPath();
			ExportCommand = new CommunityToolkit.Mvvm.Input.AsyncRelayCommand(ExportAsync, CanExport);
			BrowseFolderCommand = new RelayCommand(BrowseFolder);
			RetryCommand = new CommunityToolkit.Mvvm.Input.AsyncRelayCommand(ExportAsync, CanExport);
			NewExperimentCommand = new RelayCommand(NewExperiment);
			QuitCommand = new RelayCommand(Quit);

			ResetState();
		}
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
			set { _exportPath = value; OnPropertyChanged(); }
		}

		private string _errorMessage;
		public string ErrorMessage
		{
			get => _errorMessage;
			set { _errorMessage = value; OnPropertyChanged(); }
		}
		private void ResetState()
		{
			IsExporting = false;
			ExportSuccess = false;
			ExportError = false;
			ErrorMessage = string.Empty;
		}

		private bool CanExport() => !string.IsNullOrWhiteSpace(ExportPath) && !IsExporting;

		private async Task ExportAsync()
		{
			ResetState();
			IsExporting = true;
			try
			{
				// Utilise le dossier sélectionné dans _settings.ExportFolderPath
				_exportationService.LoadExportFolderPath();
				await _exportationService.ExportDataAsync();
				IsExporting = false;
				ExportSuccess = true;
			}
			catch (Exception ex)
			{
				IsExporting = false;
				ExportError = true;
				ErrorMessage = $"L’export a échoué.\n{ex.Message}";
			}
		}

		private void BrowseFolder()
		{
			var dlg = new System.Windows.Forms.FolderBrowserDialog
			{
				SelectedPath = ExportPath,
				Description = "Sélectionnez le dossier d’exportation"
			};
			if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				_exportationService.SaveExportFolderPath(dlg.SelectedPath);
				var NewExportPath = _exportationService.LoadExportFolderPath();
				_settings.ExportFolderPath = NewExportPath;
				ExportPath = NewExportPath;
			}

		}

		private void NewExperiment()
		{
			_parentWindow.DialogResult = true;
			_parentWindow.Close();
		}

		private void Quit()
		{
			_parentWindow.Close();
		}
	}
}