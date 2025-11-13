using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using DocumentFormat.OpenXml.Wordprocessing;

using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services.Exportation;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.Views;

namespace StroopApp.ViewModels.Experiment.Experimenter.End
{
	internal class ExportEndExperimentWindowViewModel : ViewModelBase
	{
		private readonly ExperimentSettings _settings;
		private readonly IExportationService _exportationService;
		private readonly Window _parentWindow;
		private readonly INavigationService _navigationService;
		private readonly IWindowManager _windowManager;

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

		public ExportEndExperimentWindowViewModel(ExperimentSettings settings, IExportationService exportationService, Window parentWindow, INavigationService navigationService, IWindowManager windowManager)
		{
			_settings = settings;
			_exportationService = exportationService;
			_parentWindow = parentWindow;
			_navigationService = navigationService;
			_windowManager = windowManager;

			ExportPath = _exportationService.LoadExportFolderPath();
			ExportCommand = new CommunityToolkit.Mvvm.Input.AsyncRelayCommand(ExportAsync, CanExport);
			CancelCommand = new RelayCommand(CancelExport);
			RetryCommand = new CommunityToolkit.Mvvm.Input.AsyncRelayCommand(ExportAsync, CanExport);
			OpenAndSelectTodayExportFileCommand = new RelayCommand(OpenAndSelectTodayExportFile);
			NewExperimentCommand = new RelayCommand(NewExperiment);
			QuitCommand = new RelayCommand(Quit);
			ReExportCommand = new RelayCommand(ReExport);

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
				ErrorMessage = $"L’export a échoué.\n{ex.Message}";
			}
		}
		private void OpenAndSelectTodayExportFile()
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
				MessageBox.Show("Le dossier ou le fichier n'existe pas.");
			}
		}

		private void NewExperiment()
		{
			_settings.Reset();
			_parentWindow.DialogResult = true;
			_parentWindow.Close();
			_navigationService.NavigateTo(() => new ConfigurationPage(_settings, _navigationService, _windowManager));

		}

		private void Quit()
		{
			Application.Current.Shutdown();
		}
	}
}