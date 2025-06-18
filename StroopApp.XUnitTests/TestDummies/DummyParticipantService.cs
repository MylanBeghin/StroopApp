using System.Collections.ObjectModel;

using StroopApp.Models;
using StroopApp.Services.Participant;

namespace StroopApp.XUnitTests.TestDummies
{
	public class DummyParticipantService : IParticipantService
	{
		public ObservableCollection<Participant> LoadParticipants() => new ObservableCollection<Participant>();
		public void SaveParticipants(ObservableCollection<Participant> participants)
		{
		}
		public void AddParticipant(ObservableCollection<Participant> participants, Participant participant)
		{
		}
		public void UpdateParticipantById(string id, Participant modified, ObservableCollection<Participant> list)
		{
		}
		public void DeleteParticipant(ObservableCollection<Participant> participants, string participantId)
		{
		}
	}
}
