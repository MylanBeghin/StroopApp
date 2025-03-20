using System.Collections.ObjectModel;
using StroopApp.Models;

namespace StroopApp.Services.Participant
{
    public interface IParticipantService
    {
        ObservableCollection<ParticipantModel> LoadParticipants();
        void SaveParticipants(ObservableCollection<ParticipantModel> participants);
        void DeleteParticipant(ParticipantModel participant, ObservableCollection<ParticipantModel> participants);
        int GetNextParticipantId();
    }
}
