using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;

using StroopApp.Models;
using StroopApp.Services.Participant;

namespace StroopApp.Services.Participants
{
	public class ParticipantService : IParticipantService
	{
		private readonly string _configDir;
		private readonly string _participantsPath;
		private readonly string _exportRootDirectory;

		// on récupère à la fois le configDir et les settings pour connaître ExportFolderPath
		public ParticipantService(string configDir, ExperimentSettings settings)
		{
			_configDir = configDir;
			_participantsPath = Path.Combine(_configDir, "participants.json");
			_exportRootDirectory = settings.ExportFolderPath;
		}

		public ObservableCollection<Models.Participant> LoadParticipants()
		{
			if (!File.Exists(_participantsPath))
				return new ObservableCollection<Models.Participant>();

			var json = File.ReadAllText(_participantsPath);
			return JsonSerializer.Deserialize<ObservableCollection<Models.Participant>>(json)
				?? new ObservableCollection<Models.Participant>();
		}

		public void SaveParticipants(ObservableCollection<Models.Participant> participants)
		{
			Directory.CreateDirectory(_configDir);
			var json = JsonSerializer.Serialize(participants, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(_participantsPath, json);
		}

		public void AddParticipant(ObservableCollection<Models.Participant> participants, Models.Participant participant)
		{
			participants.Add(participant);
			SaveParticipants(participants);
		}

		public void UpdateParticipantById(string id, Models.Participant modified, ObservableCollection<Models.Participant> list)
		{
			var target = list.FirstOrDefault(p => p.Id == id);
			if (target == null)
				return;
			target.CopyPropertiesFrom(modified);
			SaveParticipants(list);
		}

		public void DeleteParticipant(ObservableCollection<Models.Participant> participants, string participantId)
		{
			var toRemove = participants.FirstOrDefault(x => x.Id == participantId);
			if (toRemove == null)
				return;

			// 1) Retire et sauve le JSON
			participants.Remove(toRemove);
			SaveParticipants(participants);

			// 2) Déplace le dossier de résultats
			var resultsDir = Path.Combine(_exportRootDirectory, "Results", participantId);
			var archivedDir = Path.Combine(_exportRootDirectory, "Archived", participantId);

			if (Directory.Exists(resultsDir))
			{
				Directory.CreateDirectory(Path.Combine(_exportRootDirectory, "Archived"));
				// si déjà un dossier archivé pour ce participant, on le supprime pour écraser
				if (Directory.Exists(archivedDir))
					Directory.Delete(archivedDir, recursive: true);

				Directory.Move(resultsDir, archivedDir);
			}
		}
	}
}
