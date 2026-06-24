using ClosedXML.Excel;
using StroopApp.Models;
using StroopApp.Models.Simon;
using StroopApp.Services.Language;

namespace StroopApp.Services.Exportation
{
    public class SimonTrialExportFormatter : TrialExportFormatter
    {
        public SimonTrialExportFormatter(ILanguageService languageService) : base(languageService) { }

        public override TaskType TaskType => TaskType.Simon;

        public override List<string> GetColumnHeaders()
        {
            var headers = GetCommonPrefixHeaders();
            headers.Add(LanguageService.GetLocalizedString("Header_Congruence"));
            headers.Add(LanguageService.GetLocalizedString("Header_StimulusPosition"));
            headers.Add(LanguageService.GetLocalizedString("Header_StimulusColor"));
            headers.Add(LanguageService.GetLocalizedString("Header_Expected_Answer"));
            headers.Add(LanguageService.GetLocalizedString("Header_Given_Answer"));
            headers.AddRange(GetCommonSuffixHeaders());
            return headers;
        }

        public override void WriteRow(IXLWorksheet worksheet, int row, ITrial trial, string profileName, int blockNumber)
        {
            WriteCommonPrefix(worksheet, row, trial, profileName, blockNumber);
            SimonTrial simonTrial = (SimonTrial)trial;
            int col = CommonPrefixCount + 1;
            worksheet.Cell(row, col++).Value = simonTrial.IsCongruent;
            worksheet.Cell(row, col++).Value = simonTrial.Stimulus.Position.ToString();
            worksheet.Cell(row, col++).Value = simonTrial.Stimulus.Color.ToString();
            worksheet.Cell(row, col++).Value = simonTrial.ExpectedAnswer.ToString();
            worksheet.Cell(row, col++).Value = simonTrial.GivenAnswer.ToString();
            WriteCommonSuffix(worksheet, row, col, trial);
        }
    }
}
