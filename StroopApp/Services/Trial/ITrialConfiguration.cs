namespace StroopApp.Services.Trial
{
	/// <summary>
	/// Minimal interface for trial generation configuration.
	/// </summary>
	public interface ITrialConfiguration
	{
		/// <summary>
		/// Current block number.
		/// </summary>
		int Block { get; }

		/// <summary>
		/// Participant identifier.
		/// </summary>
		string ParticipantId { get; }

		/// <summary>
		/// Total number of words/trials to generate.
		/// </summary>
		int WordCount { get; }

		/// <summary>
		/// Percentage of congruent trials (0-100).
		/// </summary>
		int CongruencePercent { get; }

		/// <summary>
		/// Percentage for dominant amorce type (used as SwitchPercent).
		/// </summary>
		int DominantPercent { get; }

		/// <summary>
		/// Whether visual cue (amorce) is enabled.
		/// </summary>
		bool IsAmorce { get; }

		/// <summary>
		/// Language code for task stimuli (e.g., "en", "fr").
		/// </summary>
		string TaskLanguage { get; }
	}
}
