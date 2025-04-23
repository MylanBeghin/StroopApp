using System;
using System.IO;
using System.Threading.Tasks;
using ClosedXML.Excel;
using StroopApp.Models;

namespace StroopApp.Services.Exportation
{
    public class ExportationService : IExportationService
    {
        readonly ExperimentSettings _settings;

        public ExportationService(ExperimentSettings settings)
        {
            _settings = settings;
        }

        public async Task ExportDataAsync()
        {
            var participant = _settings.Participant;
            var profile = _settings.CurrentProfile;
            var block = _settings.Block;
            var results = _settings.ExperimentContext.TrialRecords;

            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "StroopExports");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var filePath = Path.Combine(
                folder,
                $"{participant.Id}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Export");

            ws.Cell(1, 1).Value = "Participant";
            ws.Cell(1, 2).Value = "Profil";
            ws.Cell(1, 3).Value = "Bloc";
            ws.Cell(1, 4).Value = "Essai";
            ws.Cell(1, 5).Value = "Temps(ms)";

            var row = 2;
            foreach (var r in results)
            {
                ws.Cell(row, 1).Value = participant.Id;
                ws.Cell(row, 2).Value = profile.ProfileName;
                ws.Cell(row, 3).Value = block;
                ws.Cell(row, 4).Value = r.TrialNumber;
                ws.Cell(row, 5).Value = r.ReactionTime;
                row++;
            }

            workbook.SaveAs(filePath);
            await Task.CompletedTask;
        }
    }
}
