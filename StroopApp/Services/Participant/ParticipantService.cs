using StroopApp.Models;
using ParticipantModel = StroopApp.Models.Participant;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StroopApp.Services.Participant
{
    /// <summary>
    /// Service for managing participant persistence (CRUD operations) and archiving deleted participant data.
    /// </summary>
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

        /// <summary>
        /// Loads all participants from JSON configuration file.
        /// </summary>
        public ObservableCollection<ParticipantModel> LoadParticipants()
        {
            if (!File.Exists(_participantsPath))
                return new ObservableCollection<ParticipantModel>();

            var json = File.ReadAllText(_participantsPath);
            return JsonSerializer.Deserialize<ObservableCollection<ParticipantModel>>(json, _jsonOptions)
                ?? new ObservableCollection<ParticipantModel>();
        }

        /// <summary>
        /// Saves all participants to JSON configuration file.
        /// </summary>
        public void SaveParticipants(ObservableCollection<ParticipantModel> participants)
        {
            Directory.CreateDirectory(_configDir);
            var json = JsonSerializer.Serialize(participants, _jsonOptions);
            File.WriteAllText(_participantsPath, json);
        }

        /// <summary>
        /// Adds a new participant and persists the collection.
        /// </summary>
        public void AddParticipant(ObservableCollection<ParticipantModel> participants, ParticipantModel participant)
        {
            participants.Add(participant);
            SaveParticipants(participants);
        }

        /// <summary>
        /// Updates an existing participant's properties and persists changes.
        /// </summary>
        public void UpdateParticipant(ParticipantModel original, ParticipantModel modified, ObservableCollection<ParticipantModel> list)
        {
            if (original == null || modified == null)
                return;

            original.CopyPropertiesFrom(modified);
            SaveParticipants(list);
        }

        /// <summary>
        /// Deletes a participant and archives their result files to the Archived folder.
        /// </summary>
        public void DeleteParticipant(ObservableCollection<ParticipantModel> participants, string participantId)
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
