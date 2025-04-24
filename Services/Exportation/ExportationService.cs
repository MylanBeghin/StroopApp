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

            ws.Cell(1, 1).Value = "Numéro du participant";
            ws.Cell(1, 2).Value = "Type de Stroop";
            ws.Cell(1, 3).Value = "Bloc";
            ws.Cell(1, 4).Value = "Réponse attendue";
            ws.Cell(1, 5).Value = "Réponse donnée";
            ws.Cell(1, 6).Value = "Validité de la réponse";
            ws.Cell(1, 7).Value = "Temps de réaction";
            ws.Cell(1, 8).Value = "Essai";
            ws.Cell(1, 9).Value = "Amorce";

            var row = 2;
            foreach (var r in results)
            {
                ws.Cell(row, 1).Value = participant.Id;
                ws.Cell(row, 2).Value = r.StroopType;
                ws.Cell(row, 3).Value = r.Block;
                ws.Cell(row, 4).Value = r.ExpectedAnswer;
                ws.Cell(row, 5).Value = r.GivenAnswer;
                ws.Cell(row, 6).Value = r.IsValidResponse;
                ws.Cell(row, 7).Value = r.ReactionTime;
                ws.Cell(row, 8).Value = r.TrialNumber;
                ws.Cell(row, 9).Value = r.Amorce switch
                {
                    AmorceType.Square => "Carré",
                    AmorceType.Round => "Cercle",
                    _ => ""
                }; ;
                row++;
            }

            workbook.SaveAs(filePath);
            await Task.CompletedTask;
        }
    }
}
