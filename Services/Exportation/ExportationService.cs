using ClosedXML.Excel;
using StroopApp.Models;
using StroopApp.Services.Participants;
using System.IO;

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
            var blocks = _settings.ExperimentContext.Blocks;
            var exeDir = AppDomain.CurrentDomain.BaseDirectory;
            var baseFolder = Path.Combine(exeDir, ParticipantService.ResultsDir);
            Directory.CreateDirectory(baseFolder);
            var participantFolder = Path.Combine(baseFolder, p.Id.ToString());
            Directory.CreateDirectory(participantFolder);

            var fileName = $"{p.Id}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";
            var filePath = Path.Combine(participantFolder, fileName);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Export");

            // en-têtes
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
            foreach (var block in blocks)
            {
                foreach (var r in block.TrialRecords)
                {
                    worksheet.Cell(row, 1).Value = p.Id;
                    worksheet.Cell(row, 2).Value = block.StroopType;
                    worksheet.Cell(row, 3).Value = block.BlockNumber;
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
            }

            workbook.SaveAs(filePath);
            await Task.CompletedTask;
        }

    }
}
