using StroopApp.Services.Participant;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace StroopApp.Services.Participants
{
    public class ParticipantService : IParticipantService
    {
        private readonly string _configDir;
        private readonly string _participantsPath;
        public ParticipantService(string configDir)
        {
            _configDir = configDir;
            _participantsPath = Path.Combine(_configDir, "participants.json");
        }

        public ObservableCollection<Models.Participant> LoadParticipants()
        {
            if (!File.Exists(_participantsPath))
                return new ObservableCollection<Models.Participant>();

            var json = File.ReadAllText(_participantsPath);
            return JsonSerializer.Deserialize<ObservableCollection<Models.Participant>>(json) ?? new ObservableCollection<Models.Participant>();
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
        public void UpdateParticipantById(int id, Models.Participant modified, ObservableCollection<Models.Participant> list)
        {
            var target = list.FirstOrDefault(p => p.Id == id);
            if (target == null)
                return;
            target.CopyPropertiesFrom(modified);
            SaveParticipants(list);
        }
        public void DeleteParticipant(ObservableCollection<Models.Participant> participants, int participantId)
        {
            var toRemove = participants.FirstOrDefault(x => x.Id == participantId);
            if (toRemove != null)
            {
                participants.Remove(toRemove);
                SaveParticipants(participants);
            }
        }
        public int GetNextParticipantId()
        {
            var list = LoadParticipants();
            return list.Count == 0 ? 1 : list.Max(x => x.Id) + 1;
        }
    }
}
