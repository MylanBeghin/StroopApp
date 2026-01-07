namespace StroopApp.Models
{
	/// <summary>
	/// Minimal interface for block configuration.
	/// Decouples SharedExperimentData and Block from ExperimentSettings god object.
	/// </summary>
	public interface IBlockConfiguration
	{
		/// <summary>
		/// Current block number.
		/// </summary>
		int Block { get; }

		/// <summary>
		/// Total number of words/trials in this block.
		/// </summary>
		int WordCount { get; }

		/// <summary>
		/// Name of the experiment profile used for this block.
		/// </summary>
		string ProfileName { get; }

		/// <summary>
		/// Percentage of congruent trials (0-100).
		/// </summary>
		int CongruencePercent { get; }

		/// <summary>
		/// Percentage for amorce switching (nullable if not applicable).
		/// </summary>
		int? SwitchPercent { get; }

		/// <summary>
		/// Whether visual cue (amorce) is enabled.
		/// </summary>
		bool IsAmorce { get; }
	}
}
