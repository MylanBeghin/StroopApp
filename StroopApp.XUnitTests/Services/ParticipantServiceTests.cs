using StroopApp.Models;
using StroopApp.Services.Participant;
using System.Collections.ObjectModel;
using Xunit;

namespace StroopApp.XUnitTests.Services
{
	public class ParticipantServiceTests
	{
		private string CreateTempDirectory()
		{
			var path = Path.Combine(Path.GetTempPath(), "ParticipantTest_" + Guid.NewGuid());
			Directory.CreateDirectory(path);
			return path;
		}

		private ExperimentSettings CreateMockSettings(string root)
			=> new ExperimentSettings { ExportFolderPath = root };

		private Participant NewParticipant(string id, int? age = null)
		{
			var p = new Participant { Id = id };
			if (age.HasValue)
				p.Age = age;
			return p;
		}

		[Fact]
		public void LoadParticipants_NoFile_ReturnsEmpty()
		{
			// Arrange & Act
			var svc = new ParticipantService(CreateTempDirectory(), CreateMockSettings(CreateTempDirectory()));
			var list = svc.LoadParticipants();

			// Assert
			Assert.Empty(list);
		}

		[Fact]
		public void SaveParticipants_WithList_Persists()
		{
			// Arrange
			var dir = CreateTempDirectory();
			var svc = new ParticipantService(dir, CreateMockSettings(dir));
			var list = new ObservableCollection<Participant> { NewParticipant("p1"), NewParticipant("p2") };

			// Act
			svc.SaveParticipants(list);

			// Assert
			var loaded = svc.LoadParticipants();
			Assert.Equal(2, loaded.Count);
		}

		[Fact]
		public void AddParticipant_AppendsAndPersists()
		{
			// Arrange
			var dir = CreateTempDirectory();
			var svc = new ParticipantService(dir, CreateMockSettings(dir));
			var list = new ObservableCollection<Participant>();

			// Act
			svc.AddParticipant(list, NewParticipant("new"));

			// Assert
			Assert.Single(list);
			Assert.Equal("new", list[0].Id);
		}

		[Fact]
		public void UpdateParticipant_Existing_UpdatesAndPersists()
		{
			// Arrange
			var dir = CreateTempDirectory();
			var svc = new ParticipantService(dir, CreateMockSettings(dir));
			var original = NewParticipant("p1", 20);
			var list = new ObservableCollection<Participant> { original };
			svc.SaveParticipants(list);

			// Act
			svc.UpdateParticipant(original, NewParticipant("p1", 35), list);

			// Assert
			var loaded = svc.LoadParticipants();
			Assert.Equal(35, loaded.First(p => p.Id == "p1").Age);
		}

		[Fact]
		public void UpdateParticipant_NullOriginal_NoChange()
		{
			// Arrange
			var dir = CreateTempDirectory();
			var svc = new ParticipantService(dir, CreateMockSettings(dir));
			var list = new ObservableCollection<Participant> { NewParticipant("p1", 20) };
			svc.SaveParticipants(list);

			// Act
			svc.UpdateParticipant(null, NewParticipant("xxx", 50), list);

			// Assert
			var loaded = svc.LoadParticipants();
			Assert.Single(loaded);
			Assert.Equal(20, loaded[0].Age);
		}

		[Fact]
		public void DeleteParticipant_WithResults_MovesToArchived()
		{
			// Arrange
			var dir = CreateTempDirectory();
			var svc = new ParticipantService(dir, CreateMockSettings(dir));
			var list = new ObservableCollection<Participant> { NewParticipant("p1") };
			svc.SaveParticipants(list);
			var results = Path.Combine(dir, "Results", "p1");
			Directory.CreateDirectory(results);
			File.WriteAllText(Path.Combine(results, "x.txt"), "x");

			// Act
			svc.DeleteParticipant(list, "p1");

			// Assert
			Assert.Empty(svc.LoadParticipants());
			Assert.False(Directory.Exists(results));
			Assert.True(File.Exists(Path.Combine(dir, "Archived", "p1", "x.txt")));
		}

		[Fact]
		public void DeleteParticipant_WithoutResults_RemovesOnlyParticipant()
		{
			// Arrange
			var dir = CreateTempDirectory();
			var svc = new ParticipantService(dir, CreateMockSettings(dir));
			var list = new ObservableCollection<Participant> { NewParticipant("p2") };
			svc.SaveParticipants(list);

			// Act
			svc.DeleteParticipant(list, "p2");

			// Assert
			Assert.Empty(svc.LoadParticipants());
			Assert.False(Directory.Exists(Path.Combine(dir, "Archived", "p2")));
		}
	}
}
