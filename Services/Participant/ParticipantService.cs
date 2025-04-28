using StroopApp.Services.Participant;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace StroopApp.Services.Participants
{
    public class ParticipantService : IParticipantService
    {
        readonly string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        readonly string jsonPath;
        public const string ResultsDir = "Results";
        public const string DeletedDir = "Deleted";

        public ParticipantService()
        {
            Directory.CreateDirectory(baseDir);
            jsonPath = Path.Combine(baseDir, "participants.json");
        }

        public ObservableCollection<Models.Participant> LoadParticipants()
        {
            if (!File.Exists(jsonPath))
                return new ObservableCollection<Models.Participant>();
            var json = File.ReadAllText(jsonPath);
            return JsonSerializer.Deserialize<ObservableCollection<Models.Participant>>(json);
        }

        public void SaveParticipants(ObservableCollection<Models.Participant> participants)
        {
            var json = JsonSerializer.Serialize(participants, new JsonSerializerOptions { WriteIndented = true });

            // Avant d’écrire, s’assurer de retirer tout attribut en lecture seule ou caché
            if (File.Exists(jsonPath))
                File.SetAttributes(jsonPath, FileAttributes.Normal);

            File.WriteAllText(jsonPath, json);

            // Puis masquer le fichier une fois écrit
            File.SetAttributes(jsonPath, File.GetAttributes(jsonPath) | FileAttributes.Hidden);

            var resultsRoot = Path.Combine(baseDir, ResultsDir);
            Directory.CreateDirectory(resultsRoot);
            foreach (var p in participants)
            {
                var dir = Path.Combine(resultsRoot, p.Id.ToString());
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
        }


        public void DeleteParticipant(int participantId)
        {
            var participants = LoadParticipants();
            var p = participants.FirstOrDefault(x => x.Id == participantId);
            if (p == null) return;

            var src = Path.Combine(baseDir, ResultsDir, participantId.ToString());
            if (Directory.Exists(src))
            {
                var dstRoot = Path.Combine(baseDir, DeletedDir);
                Directory.CreateDirectory(dstRoot);

                var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                var folderName = $"{participantId}_{timestamp}";
                var dst = Path.Combine(dstRoot, folderName);
                var suffix = 1;
                while (Directory.Exists(dst))
                    dst = Path.Combine(dstRoot, $"{folderName}({suffix++})");
                Directory.Move(src, dst);
            }

            participants.Remove(p);
            SaveParticipants(participants);
        }

        public int GetNextParticipantId()
        {
            var list = LoadParticipants();
            return list.Count == 0 ? 1 : list.Max(x => x.Id) + 1;
        }
    }
}
