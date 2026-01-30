using System.Collections.Generic;

using StroopApp.Models;

namespace StroopApp.Services.Trial
{
    /// <summary>
    /// Defines contract for generating Stroop trial sequences with optional visual cues.
    /// </summary>
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
	List<VisualCueType> GenerateAmorceSequence(int count, int switchPercentage);
	}
}
