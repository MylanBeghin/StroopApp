using System.Collections.Generic;

using StroopApp.Models;

namespace StroopApp.Services.Trial
{
	public interface ITrialGenerationService
	{
	/// <summary>
	/// Generates a list of trials based on experiment parameters.
	/// </summary>
	/// <param name="settings">Experiment settings</param>
	/// <returns>List of generated trials</returns>
	List<StroopTrial> GenerateTrials(ExperimentSettings settings);

	/// <summary>
	/// Generates a sequence of visual cues (optional).
	/// </summary>
	/// <param name="count">Number of cues to generate</param>
	/// <param name="switchPercentage">Percentage of cue switches</param>
	/// <returns>Sequence of cues</returns>
	List<AmorceType> GenerateAmorceSequence(int count, int switchPercentage);
	}
}
