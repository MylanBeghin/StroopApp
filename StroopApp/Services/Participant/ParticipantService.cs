using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using StroopApp.Models;

namespace StroopApp.Services.Participant
{
	public class ParticipantService : IParticipantService
	{
		private readonly string _configDir;
		private readonly string _participantsPath;
		private readonly string _exportRootDirectory;

		private static readonly JsonSerializerOptions _jsonOptions = new()
		{
			WriteIndented = true,
			NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
		};

		public ParticipantService(string configDir, ExperimentSettings settings)
		{
			_configDir = configDir ?? throw new ArgumentNullException(nameof(configDir));
			ArgumentNullException.ThrowIfNull(settings);
			_participantsPath = Path.Combine(_configDir, "participants.json");
			_exportRootDirectory = settings.ExportFolderPath;
		}

		public ObservableCollection<Models.Participant> LoadParticipants()
		{
			if (!File.Exists(_participantsPath))
				return new ObservableCollection<Models.Participant>();

			var json = File.ReadAllText(_participantsPath);
			return JsonSerializer.Deserialize<ObservableCollection<Models.Participant>>(json, _jsonOptions)
				?? new ObservableCollection<Models.Participant>();
		}

		public void SaveParticipants(ObservableCollection<Models.Participant> participants)
		{
			Directory.CreateDirectory(_configDir);
			var json = JsonSerializer.Serialize(participants, _jsonOptions);
			File.WriteAllText(_participantsPath, json);
		}

		public void AddParticipant(ObservableCollection<Models.Participant> participants, Models.Participant participant)
		{
			participants.Add(participant);
			SaveParticipants(participants);
		}

		public void UpdateParticipant(Models.Participant original, Models.Participant modified, ObservableCollection<Models.Participant> list)
		{
			if (original == null || modified == null)
				return;
			
			original.CopyPropertiesFrom(modified);
			SaveParticipants(list);
		}

		public void DeleteParticipant(ObservableCollection<Models.Participant> participants, string participantId)
		{
			var toRemove = participants.FirstOrDefault(x => x.Id == participantId);
			if (toRemove == null)
				return;
			participants.Remove(toRemove);
			SaveParticipants(participants);

			var resultsDir = Path.Combine(_exportRootDirectory, "Results", participantId);
			var archivedDir = Path.Combine(_exportRootDirectory, "Archived", participantId);

			if (Directory.Exists(resultsDir))
			{
				Directory.CreateDirectory(Path.Combine(_exportRootDirectory, "Archived"));
				if (Directory.Exists(archivedDir))
					Directory.Delete(archivedDir, recursive: true);

				Directory.Move(resultsDir, archivedDir);
			}
		}
	}
}
