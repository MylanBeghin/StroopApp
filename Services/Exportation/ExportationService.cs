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
            var p = _settings.Participant;
            var profile = _settings.CurrentProfile;
            var block = _settings.Block;
            var results = _settings.ExperimentContext.TrialRecords;

            // dossier de l’exécutable
            var exeDir = AppDomain.CurrentDomain.BaseDirectory;
            var participantFolder = Path.Combine(exeDir, "StroopExports", p.Id.ToString());
            if (!Directory.Exists(participantFolder))
                Directory.CreateDirectory(participantFolder);

            // nom de fichier avec date et heure
            var fileName = $"{p.Id}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";
            var filePath = Path.Combine(participantFolder, fileName);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Export");

            worksheet.Cell(1, 1).Value = "Numéro du participant";
            worksheet.Cell(1, 2).Value = "Type de Stroop";
            worksheet.Cell(1, 3).Value = "Bloc";
            worksheet.Cell(1, 4).Value = "Réponse attendue";
            worksheet.Cell(1, 5).Value = "Réponse donnée";
            worksheet.Cell(1, 6).Value = "Validité de la réponse";
            worksheet.Cell(1, 7).Value = "Temps de réaction";
            worksheet.Cell(1, 8).Value = "Essai";
            worksheet.Cell(1, 9).Value = "Amorce";

            var row = 2;
            foreach (var r in results)
            {
                worksheet.Cell(row, 1).Value = p.Id;
                worksheet.Cell(row, 2).Value = r.StroopType;
                worksheet.Cell(row, 3).Value = r.Block;
                worksheet.Cell(row, 4).Value = r.ExpectedAnswer;
                worksheet.Cell(row, 5).Value = r.GivenAnswer;
                worksheet.Cell(row, 6).Value = r.IsValidResponse;
                worksheet.Cell(row, 7).Value = r.ReactionTime;
                worksheet.Cell(row, 8).Value = r.TrialNumber;
                worksheet.Cell(row, 9).Value = r.Amorce switch
                {
                    AmorceType.Square => "Carré",
                    AmorceType.Round => "Cercle",
                    _ => ""
                };
                row++;
            }

            workbook.SaveAs(filePath);
            await Task.CompletedTask;
        }
    }
}
