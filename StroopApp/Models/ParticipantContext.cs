namespace StroopApp.Models
{
	/// <summary>
	/// Represents participant identity context.
	/// Contains the current participant information.
	/// This is a simple POCO without logic, extracted from ExperimentSettings.
	/// </summary>
	public class ParticipantContext
	{
		/// <summary>
		/// Current participant in the experiment.
		/// </summary>
		public Participant? Participant { get; set; }

		/// <summary>
		/// Default constructor.
		/// Participant is initialized to null and must be set explicitly.
		/// </summary>
		public ParticipantContext()
		{
			Participant = null;
		}
	}
}
