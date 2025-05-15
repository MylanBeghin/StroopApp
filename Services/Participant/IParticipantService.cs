using System.Collections.ObjectModel;

namespace StroopApp.Services.Participant
{
    public interface IParticipantService
    {
        ObservableCollection<Models.Participant> LoadParticipants();
        void SaveParticipants(ObservableCollection<Models.Participant> participants);
        void AddParticipant(ObservableCollection<Models.Participant> participants, Models.Participant participant);
        void DeleteParticipant(ObservableCollection<Models.Participant> participants, int participantId);
        int GetNextParticipantId();
    }
}