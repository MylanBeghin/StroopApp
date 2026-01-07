namespace StroopApp.Models
{
	/// <summary>
	/// Represents runtime execution state for an experiment.
	/// Contains current block number and experiment context (which includes state flags).
	/// This is a simple POCO without logic, extracted from ExperimentSettings.
	/// </summary>
	public class ExperimentRunState
	{
		/// <summary>
		/// Current block number in the experiment.
		/// </summary>
		public int Block { get; set; }

		/// <summary>
		/// Shared experiment data containing trials, reaction times, and state flags.
		/// </summary>
		public SharedExperimentData ExperimentContext { get; set; }

		/// <summary>
		/// Default constructor initializing with default values.
		/// </summary>
		public ExperimentRunState()
		{
			Block = 1;
			ExperimentContext = new SharedExperimentData();
		}
	}
}
