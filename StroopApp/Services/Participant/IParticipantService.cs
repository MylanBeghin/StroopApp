using System.Collections.ObjectModel;
using ParticipantModel = StroopApp.Models.Participant;

namespace StroopApp.Services.Participant
{
	/// <summary>
	/// Service responsible for managing participant data persistence and lifecycle.
	/// </summary>
	public interface IParticipantService
	{
		/// <summary>
		/// Loads all participants from persistent storage.
		/// </summary>
		/// <returns>Collection of participants, or empty if no file exists.</returns>
		ObservableCollection<ParticipantModel> LoadParticipants();
		
		/// <summary>
		/// Persists the participant collection to storage.
		/// </summary>
		/// <param name="participants">Collection to save.</param>
		void SaveParticipants(ObservableCollection<ParticipantModel> participants);
		
		/// <summary>
		/// Adds a new participant and saves to storage.
		/// </summary>
		void AddParticipant(ObservableCollection<ParticipantModel> participants, ParticipantModel participant);
		
		/// <summary>
		/// Updates an existing participant by copying properties from modified to original.
		/// </summary>
		/// <param name="original">Original participant reference in the collection.</param>
		/// <param name="modified">Modified participant with new values.</param>
		/// <param name="list">Collection containing the participant.</param>
		void UpdateParticipant(ParticipantModel original, ParticipantModel modified, ObservableCollection<ParticipantModel> list);
		
		/// <summary>
		/// Deletes a participant and archives their experimental results.
		/// </summary>
		/// <param name="participants">Collection to remove from.</param>
		/// <param name="participantId">ID of participant to delete.</param>
		void DeleteParticipant(ObservableCollection<ParticipantModel> participants, string participantId);
	}
}
