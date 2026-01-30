using ClosedXML.Excel;
using StroopApp.Models;
using StroopApp.Services.Language;
using System.IO;
using System.Text.Json;

namespace StroopApp.Services.Exportation
{
    /// <summary>
    /// Service for exporting experiment data to Excel files with localized headers.
    /// Manages export directory configuration and file generation.
    /// </summary>
    public class ExportationService : IExportationService, IDisposable
    {
        private readonly ExperimentSettings _settings;
        private readonly string _configDir;
        private readonly string _exportFolderConfigFile;
        private string _exportRootDirectory;
        private readonly ILanguageService _languageService;
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
        public ExportationService(ExperimentSettings settings, ILanguageService languageService, string configDir)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _languageService = languageService ?? throw new ArgumentNullException(nameof(languageService));
            _configDir = configDir ?? throw new ArgumentNullException(nameof(configDir));
            Directory.CreateDirectory(configDir);
            _exportFolderConfigFile = Path.Combine(_configDir, "exportFolder.json");
            _exportRootDirectory = LoadExportFolderPath();
            _settings.ExportFolderPath = _exportRootDirectory;

            _settings.PropertyChanged += OnProfileExportPathChanged;
        }

        public void OnProfileExportPathChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_settings.ExportFolderPath))
            {
                _exportRootDirectory = _settings.ExportFolderPath;
                SaveExportFolderPath(_exportRootDirectory);
            }
        }
        /// <summary>
        /// Loads the export folder path from configuration file, or returns MyDocuments if not found.
        /// </summary>
        public string LoadExportFolderPath()
        {
            if (!File.Exists(_exportFolderConfigFile))
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var json = File.ReadAllText(_exportFolderConfigFile);
            return JsonSerializer.Deserialize<string>(json)
                   ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
        /// <summary>
        /// Saves the export folder path to configuration file.
        /// </summary>
        public void SaveExportFolderPath(string path)
        {
            File.WriteAllText(_exportFolderConfigFile, JsonSerializer.Serialize(path));
        }
        /// <summary>
        /// Exports all experiment data to an Excel file in the configured directory structure.
        /// </summary>
        /// <returns>Full path to the generated Excel file.</returns>
        public async Task<string> ExportDataAsync()
        {
            var root = _settings.ExportFolderPath;
            if (string.IsNullOrWhiteSpace(root))
                throw new InvalidOperationException("No export directory configured.");

            Directory.CreateDirectory(root);
            var resultsDir = Path.Combine(root, "Results");
            var archived = Path.Combine(root, "Archived");
            Directory.CreateDirectory(resultsDir);
            Directory.CreateDirectory(archived);

            var p = _settings.Participant;
            var dateFolder = DateTime.Now.ToString("yyyy-MM-dd");
            var partDir = Path.Combine(resultsDir, p.Id, dateFolder);
            Directory.CreateDirectory(partDir);

            var ts = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var filePath = Path.Combine(partDir, $"{p.Id}_{ts}.xlsx");

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Export");
            ws.Cell(1, 1).Value = _languageService.GetLocalizedString("Header_ParticipantId");
            ws.Cell(1, 2).Value = _languageService.GetLocalizedString("Header_Congruence");
            ws.Cell(1, 3).Value = _languageService.GetLocalizedString("Header_VisualCue");
            ws.Cell(1, 4).Value = _languageService.GetLocalizedString("Header_BlockNumber");
            ws.Cell(1, 5).Value = _languageService.GetLocalizedString("Header_Expected_Answer");
            ws.Cell(1, 6).Value = _languageService.GetLocalizedString("Header_Given_Answer");
            ws.Cell(1, 7).Value = _languageService.GetLocalizedString("Header_Response_Validity");
            ws.Cell(1, 8).Value = _languageService.GetLocalizedString("Header_ResponseTime");
            ws.Cell(1, 9).Value = _languageService.GetLocalizedString("Header_Trials");
            ws.Cell(1, 10).Value = _languageService.GetLocalizedString("Header_Visual_Cue_Type");

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
                        validCell.Value = r.IsValidResponse.Value;
                    else
                        validCell.Clear();

                    ws.Cell(row, 8).Value = r.ReactionTime;
                    ws.Cell(row, 9).Value = r.TrialNumber;
                    ws.Cell(row, 10).Value = r.VisualCue switch
                    {
                        VisualCueType.Square => _languageService.GetLocalizedString("Label_Square"),
                        VisualCueType.Round => _languageService.GetLocalizedString("Label_Circle"),
                        _ => ""
                    };
                    row++;
                }


            wb.SaveAs(filePath);
            return filePath;
        }
        public void Dispose()
        {
            _settings.PropertyChanged -= OnProfileExportPathChanged;
        }
    }
}
