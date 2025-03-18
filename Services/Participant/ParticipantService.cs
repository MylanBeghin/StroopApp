using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using StroopApp.Models;

namespace StroopApp.Services.Participant
{
    public class ParticipantService : IParticipantService
    {
        private const string ParticipantFile = "participants.json";
        private const string ResultsFolder = "Résultats";

        public ObservableCollection<ParticipantModel> LoadParticipants()
        {
            if (!File.Exists(ParticipantFile))
                return new ObservableCollection<ParticipantModel>();
            var json = File.ReadAllText(ParticipantFile);
            return JsonSerializer.Deserialize<ObservableCollection<ParticipantModel>>(json);
        }

        public void SaveParticipants(ObservableCollection<ParticipantModel> participants)
        {
            var json = JsonSerializer.Serialize(participants, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ParticipantFile, json);
        }

        public void DeleteParticipant(ParticipantModel participant, ObservableCollection<ParticipantModel> participants)
        {
            if (participant != null)
            {
                participants.Remove(participant);
                SaveParticipants(participants);
            }
        }

        public int GetNextParticipantId()
        {
            var participants = LoadParticipants();
            if (participants.Count == 0)
                return 1;
            // On retourne l'ID suivant (en supposant un tri par ID croissant)
            return participants[^1].Id + 1;
        }
    }
}
