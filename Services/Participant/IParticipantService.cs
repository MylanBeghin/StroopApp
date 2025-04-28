using System.Collections.ObjectModel;
using StroopApp.Models;

namespace StroopApp.Services.Participant
{
    public interface IParticipantService
    {
        ObservableCollection<Models.Participant> LoadParticipants();
        void SaveParticipants(ObservableCollection<Models.Participant> participants);
        void DeleteParticipant(int participantId);
        int GetNextParticipantId();
    }
}