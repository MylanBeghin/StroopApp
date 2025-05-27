using System.Text.Json;

using ClosedXML.Excel;

using StroopApp.Models;
using StroopApp.Notifiers;
using StroopApp.Services.Exportation;

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

		private ExperimentSettings CreateMockSettings()
		{
			var participant = new Participant { Id = "42" };
			var block = new Block(1, "dummy");
			block.TrialRecords.Add(new StroopTrial
			{
				IsCongruent = true,
				ExpectedAnswer = "Rouge",
				GivenAnswer = "Rouge",
				IsValidResponse = true,
				ReactionTime = 500,
				TrialNumber = 1,
				Amorce = AmorceType.Square
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

		private class FakeNotifier : IUserNotifier
		{
			public Task NotifyAsync(string title, string message) => Task.CompletedTask;
		}

		[Fact]
		public async Task ExportDataAsync_CreatesDirectoriesAndExcel()
		{
			// Arrange
			var tempDir = CreateTempDirectory();
			var settings = CreateMockSettings();
			var service = new ExportationService(settings, tempDir, new FakeNotifier())
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
			Assert.Equal("Numéro du participant", ws.Cell(1, 1).Value);
			Assert.Equal(settings.Participant.Id, ws.Cell(2, 1).Value);
			Assert.Equal("Carré", ws.Cell(2, 10).Value);
		}

		[Fact]
		public void ExportRootDirectory_SetValue_CreatesConfigFile()
		{
			// Arrange
			var tempDir = CreateTempDirectory();
			var configDir = Path.Combine(tempDir, "Config");
			Directory.CreateDirectory(configDir);
			var settings = CreateMockSettings();
			var service = new ExportationService(settings, configDir, new FakeNotifier());

			// Act
			var custom = @"C:\Dossier\Test";
			service.ExportRootDirectory = custom;

			// Assert
			var cfg = Path.Combine(configDir, "exportFolder.txt");
			Assert.True(File.Exists(cfg));
			var loaded = JsonSerializer.Deserialize<string>(File.ReadAllText(cfg));
			Assert.Equal(custom, loaded);
		}

		[Fact]
		public async Task ExportDataAsync_EmptyRoot_Throws()
		{
			// Arrange
			var service = new ExportationService(CreateMockSettings(), CreateTempDirectory(), new FakeNotifier())
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
			var block2 = new Block(2, "dummy2");
			block2.TrialRecords.Add(new StroopTrial { IsCongruent = false, ExpectedAnswer = "A", GivenAnswer = "B", IsValidResponse = false, ReactionTime = 750, TrialNumber = 1, Amorce = AmorceType.Round });
			block2.TrialRecords.Add(new StroopTrial { IsCongruent = true, ExpectedAnswer = "C", GivenAnswer = "C", IsValidResponse = true, ReactionTime = 300, TrialNumber = 2, Amorce = AmorceType.Square });
			settings.ExperimentContext.Blocks.Add(block2);
			var service = new ExportationService(settings, tempDir, new FakeNotifier())
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
			Assert.Equal("Cercle", ws.Cell(3, 10).Value);
		}

		[Fact]
		public async Task ExportDataAsync_CreatesResultsAndArchivedDirectories()
		{
			// Arrange
			var tempDir = CreateTempDirectory();
			var service = new ExportationService(CreateMockSettings(), tempDir, new FakeNotifier())
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
			var service = new ExportationService(CreateMockSettings(), tempDir, new FakeNotifier())
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
	}
}
