using System.IO;
using System.Text.Json;

using ClosedXML.Excel;

using ModernWpf.Controls;

using StroopApp.Models;
using StroopApp.Notifiers;

namespace StroopApp.Services.Exportation
{
	public class ExportationService : IExportationService
	{
		private readonly ExperimentSettings _settings;
		private readonly string _configDir;
		private readonly string _exportFolderConfigFile;
		private string _exportRootDirectory;
		private readonly IUserNotifier _notifier;
		public string ExportRootDirectory
		{
			get => _exportRootDirectory;
			set
			{
				if (_exportRootDirectory != value)
				{
					_exportRootDirectory = value;
					_settings.ExportFolderPath = value;
					SaveExportFolderPath(value);
				}
			}
		}
		public ExportationService(ExperimentSettings settings, string configDir, IUserNotifier notifier = null)
		{
			_settings = settings;
			_configDir = configDir;
			Directory.CreateDirectory(configDir);
			_exportFolderConfigFile = Path.Combine(_configDir, "exportFolder.json");
			_exportRootDirectory = LoadExportFolderPath();
			_settings.ExportFolderPath = _exportRootDirectory;

			_notifier = notifier ?? new DefaultNotifier();

			_settings.CurrentProfile.PropertyChanged += OnProfileExportPathChanged;
		}

		public void OnProfileExportPathChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(_settings.ExportFolderPath))
			{
				_exportRootDirectory = _settings.ExportFolderPath;
				SaveExportFolderPath(_exportRootDirectory);
			}
		}
		public string LoadExportFolderPath()
		{
			if (!File.Exists(_exportFolderConfigFile))
				return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var json = File.ReadAllText(_exportFolderConfigFile);
			return JsonSerializer.Deserialize<string>(json)
				   ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		}
		public void SaveExportFolderPath(string path)
		{
			File.WriteAllText(_exportFolderConfigFile, JsonSerializer.Serialize(path));
		}
		public async Task<string> ExportDataAsync()
		{
			var root = _settings.ExportFolderPath;
			if (string.IsNullOrWhiteSpace(root))
				throw new InvalidOperationException("Aucun dossier d'export configuré.");

			Directory.CreateDirectory(root);
			var resultsDir = Path.Combine(root, "Results");
			var archived = Path.Combine(root, "Archived");
			Directory.CreateDirectory(resultsDir);
			Directory.CreateDirectory(archived);

			var p = _settings.Participant;
			var dateFolder = DateTime.Now.ToString("yyyy-MM-dd");
			var partDir = Path.Combine(resultsDir, p.Id.ToString(), dateFolder); // dossier par participant + jour
			Directory.CreateDirectory(partDir);

			var ts = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
			var filePath = Path.Combine(partDir, $"{p.Id}_{ts}.xlsx");

			using var wb = new XLWorkbook();
			var ws = wb.Worksheets.Add("Export");
			ws.Cell(1, 1).Value = "Numéro du participant";
			ws.Cell(1, 2).Value = "Congruence";
			ws.Cell(1, 3).Value = "Amorce ?";
			ws.Cell(1, 4).Value = "Bloc";
			ws.Cell(1, 5).Value = "Réponse attendue";
			ws.Cell(1, 6).Value = "Réponse donnée";
			ws.Cell(1, 7).Value = "Validité de la réponse";
			ws.Cell(1, 8).Value = "Temps de réaction";
			ws.Cell(1, 9).Value = "Essai";
			ws.Cell(1, 10).Value = "Type d'amorce";

			var row = 2;
			foreach (var block in _settings.ExperimentContext.Blocks)
				foreach (var r in block.TrialRecords)
				{
					ws.Cell(row, 1).Value = p.Id;
					ws.Cell(row, 2).Value = r.IsCongruent;
					ws.Cell(row, 3).Value = _settings.CurrentProfile.IsAmorce;
					ws.Cell(row, 4).Value = block.BlockNumber;
					ws.Cell(row, 5).Value = r.ExpectedAnswer;
					ws.Cell(row, 6).Value = r.GivenAnswer;

					var validCell = ws.Cell(row, 7);
					if (r.IsValidResponse.HasValue)
						validCell.Value = r.IsValidResponse.Value;   // true ou false
					else
						validCell.Clear();                           // pas de réponse => cellule vide

					ws.Cell(row, 8).Value = r.ReactionTime;
					ws.Cell(row, 9).Value = r.TrialNumber;
					ws.Cell(row, 10).Value = r.Amorce switch
					{
						AmorceType.Square => "Carré",
						AmorceType.Round => "Cercle",
						_ => ""
					};
					row++;
				}


			wb.SaveAs(filePath);
			return filePath;
		}
		public void Dispose()
		{
			_settings.CurrentProfile.PropertyChanged -= OnProfileExportPathChanged;
		}
	}
}
