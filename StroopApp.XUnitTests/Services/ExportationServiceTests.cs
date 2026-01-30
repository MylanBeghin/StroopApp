using ClosedXML.Excel;
using StroopApp.Models;
using StroopApp.Services.Exportation;
using StroopApp.Services.Language;
using System.Text.Json;
using Xunit;

namespace StroopApp.XUnitTests.Services
{
    public class ExportationServiceTests
    {
        private string CreateTempDirectory()
        {
            var path = Path.Combine(Path.GetTempPath(), "ExportTest_" + Guid.NewGuid());
            Directory.CreateDirectory(path);
            return path;
        }
        ExperimentSettings Settings = new ExperimentSettings
        {
            Participant = new Participant { Id = "42" },
            ExperimentContext = new SharedExperimentData(),
            CurrentProfile = new ExperimentProfile { IsAmorce = true }
        };
        private ExperimentSettings CreateMockSettings()
        {
            var participant = new Participant { Id = "42" };
            var block = new Block(Settings);
            block.TrialRecords.Add(new StroopTrial
            {
                IsCongruent = true,
                ExpectedAnswer = "Rouge",
                GivenAnswer = "Bleu",
                IsValidResponse = true,
                ReactionTime = 500,
                TrialNumber = 1,
                VisualCue = VisualCueType.Square
            });
            var ctx = new SharedExperimentData();
            ctx.Blocks.Add(block);
            var profile = new ExperimentProfile { IsAmorce = true };
            return new ExperimentSettings
            {
                Participant = participant,
                ExperimentContext = ctx,
                CurrentProfile = profile
            };
        }

        private ILanguageService CreateLanguageService(string languageCode = "fr") => new FakeLanguageService(languageCode);

        private class FakeLanguageService : ILanguageService
        {
            private readonly Dictionary<string, Dictionary<string, string>> _translations = new()
            {
                ["fr"] = new()
                {
                    ["Header_ParticipantId"] = "ID du participant",
                    ["Header_Congruence"] = "Congruence",
                    ["Header_VisualCue"] = "Indice visuel",
                    ["Header_BlockNumber"] = "Bloc",
                    ["Header_Expected_Answer"] = "Réponse attendue",
                    ["Header_Given_Answer"] = "Réponse donnée",
                    ["Header_Response_Validity"] = "Validité de la réponse",
                    ["Header_ResponseTime"] = "Temps de réponse",
                    ["Header_Trials"] = "Essais",
                    ["Header_Visual_Cue_Type"] = "Type d'indice visuel",
                    ["Label_Square"] = "Carré",
                    ["Label_Circle"] = "Cercle"
                },
                ["en"] = new()
                {
                    ["Header_ParticipantId"] = "Participant ID",
                    ["Header_Congruence"] = "Congruence",
                    ["Header_VisualCue"] = "Visual cue",
                    ["Header_BlockNumber"] = "Block",
                    ["Header_Expected_Answer"] = "Expected answer",
                    ["Header_Given_Answer"] = "Given answer",
                    ["Header_Response_Validity"] = "Response validity",
                    ["Header_ResponseTime"] = "Response time",
                    ["Header_Trials"] = "Trials",
                    ["Header_Visual_Cue_Type"] = "Visual cue type",
                    ["Label_Square"] = "Square",
                    ["Label_Circle"] = "Circle"
                }
            };

            public FakeLanguageService(string languageCode = "fr")
            {
                CurrentLanguageCode = languageCode;
            }

            public string CurrentLanguageCode { get; private set; }

            public string GetLocalizedString(string resourceKey, string? cultureCode = null)
            {
                var language = cultureCode ?? CurrentLanguageCode;

                if (_translations.TryGetValue(language, out var values) && values.TryGetValue(resourceKey, out var translation))
                    return translation;

                if (_translations.TryGetValue("en", out var fallback) && fallback.TryGetValue(resourceKey, out var fallbackTranslation))
                    return fallbackTranslation;

                return resourceKey;
            }

            public void SetLanguage(string languageCode)
            {
                if (_translations.ContainsKey(languageCode))
                    CurrentLanguageCode = languageCode;
            }
        }

        [Fact]
        public async Task ExportDataAsync_CreatesDirectoriesAndExcel()
        {
            // Arrange
            var tempDir = CreateTempDirectory();
            var settings = CreateMockSettings();
            var languageService = CreateLanguageService();
            var service = new ExportationService(settings, languageService, tempDir)
            {
                ExportRootDirectory = tempDir
            };

            // Act
            var filePath = await service.ExportDataAsync();

            // Assert
            Assert.True(File.Exists(filePath));
            var dayDir = Path.Combine(tempDir, "Results", settings.Participant.Id, DateTime.Now.ToString("yyyy-MM-dd"));
            Assert.True(Directory.Exists(dayDir));
            using var wb = new XLWorkbook(filePath);
            var ws = wb.Worksheet("Export");
            Assert.Equal(languageService.GetLocalizedString("Header_ParticipantId"), ws.Cell(1, 1).Value);
            Assert.Equal(settings.Participant.Id, ws.Cell(2, 1).Value);
            Assert.Equal(languageService.GetLocalizedString("Label_Square"), ws.Cell(2, 10).Value);
        }

        [Fact]
        public void ExportRootDirectory_SetValue_CreatesConfigFile()
        {
            // Arrange
            var tempDir = CreateTempDirectory();
            var configDir = Path.Combine(tempDir, "Config");
            Directory.CreateDirectory(configDir);
            var settings = CreateMockSettings();
            var service = new ExportationService(settings, CreateLanguageService(), configDir);

            // Act
            var custom = @"C:\Dossier\Test";
            service.ExportRootDirectory = custom;

            // Assert
            var cfg = Path.Combine(configDir, "exportFolder.json");
            Assert.True(File.Exists(cfg));
            var loaded = JsonSerializer.Deserialize<string>(File.ReadAllText(cfg));
            Assert.Equal(custom, loaded);
        }

        [Fact]
        public async Task ExportDataAsync_EmptyRoot_Throws()
        {
            // Arrange
            var service = new ExportationService(CreateMockSettings(), CreateLanguageService(), CreateTempDirectory())
            {
                ExportRootDirectory = ""
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.ExportDataAsync());
        }

        [Fact]
        public async Task ExportDataAsync_MultipleBlocks_WritesAllTrials()
        {
            // Arrange
            var tempDir = CreateTempDirectory();
            var settings = CreateMockSettings();
            var block2 = new Block(Settings);
            block2.TrialRecords.Add(new StroopTrial { IsCongruent = false, ExpectedAnswer = "A", GivenAnswer = "B", IsValidResponse = false, ReactionTime = 750, TrialNumber = 1, VisualCue = VisualCueType.Round });
            block2.TrialRecords.Add(new StroopTrial { IsCongruent = true, ExpectedAnswer = "C", GivenAnswer = "C", IsValidResponse = true, ReactionTime = 300, TrialNumber = 2, VisualCue = VisualCueType.Square });
            settings.ExperimentContext.Blocks.Add(block2);
            var languageService = CreateLanguageService();
            var service = new ExportationService(settings, languageService, tempDir)
            {
                ExportRootDirectory = tempDir
            };

            // Act
            var filePath = await service.ExportDataAsync();

            // Assert
            using var wb = new XLWorkbook(filePath);
            var ws = wb.Worksheet("Export");
            Assert.Equal(4, ws.LastRowUsed().RowNumber());
            Assert.False((bool)ws.Cell(3, 2).Value);
            Assert.Equal(languageService.GetLocalizedString("Label_Circle"), ws.Cell(3, 10).Value);
        }

        [Fact]
        public async Task ExportDataAsync_CreatesResultsAndArchivedDirectories()
        {
            // Arrange
            var tempDir = CreateTempDirectory();
            var service = new ExportationService(CreateMockSettings(), CreateLanguageService(), tempDir)
            {
                ExportRootDirectory = tempDir
            };

            // Act
            await service.ExportDataAsync();

            // Assert
            Assert.True(Directory.Exists(Path.Combine(tempDir, "Results")));
            Assert.True(Directory.Exists(Path.Combine(tempDir, "Archived")));
        }

        [Fact]
        public async Task ExportDataAsync_MultipleCalls_GivesUniqueFileNames()
        {
            // Arrange
            var tempDir = CreateTempDirectory();
            var service = new ExportationService(CreateMockSettings(), CreateLanguageService(), tempDir)
            {
                ExportRootDirectory = tempDir
            };

            // Act
            var f1 = await service.ExportDataAsync();
            await Task.Delay(1000);
            var f2 = await service.ExportDataAsync();

            // Assert
            Assert.NotEqual(f1, f2);
        }

        [Fact]
        public async Task ExportDataAsync_UsesCultureSpecificHeaders()
        {
            // Arrange
            var tempDir = CreateTempDirectory();

            var frConfigDir = Path.Combine(tempDir, "ConfigFr");
            var frExportDir = Path.Combine(tempDir, "ExportFr");
            var frLanguageService = CreateLanguageService("fr");
            var frSettings = CreateMockSettings();
            var frService = new ExportationService(frSettings, frLanguageService, frConfigDir)
            {
                ExportRootDirectory = frExportDir
            };

            var enConfigDir = Path.Combine(tempDir, "ConfigEn");
            var enExportDir = Path.Combine(tempDir, "ExportEn");
            var enLanguageService = CreateLanguageService("en");
            var enSettings = CreateMockSettings();
            var enService = new ExportationService(enSettings, enLanguageService, enConfigDir)
            {
                ExportRootDirectory = enExportDir
            };

            // Act
            var frFile = await frService.ExportDataAsync();
            var enFile = await enService.ExportDataAsync();

            // Assert
            using var frWorkbook = new XLWorkbook(frFile);
            using var enWorkbook = new XLWorkbook(enFile);

            var frHeader = frWorkbook.Worksheet("Export").Cell(1, 1).GetString();
            var enHeader = enWorkbook.Worksheet("Export").Cell(1, 1).GetString();

            Assert.Equal(frLanguageService.GetLocalizedString("Header_ParticipantId"), frHeader);
            Assert.Equal(enLanguageService.GetLocalizedString("Header_ParticipantId"), enHeader);
            Assert.NotEqual(frHeader, enHeader);
        }
    }
}
