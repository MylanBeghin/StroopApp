using StroopApp.Models;
using StroopApp.ViewModels.State;

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
        List<StroopTrial> GenerateTrials(ExperimentSettingsViewModel settings);

        /// <summary>
        /// Generates a sequence of visual cues (optional).
        /// </summary>
        /// <param name="count">Number of cues to generate</param>
        /// <param name="switchPercentage">Percentage of cue switches</param>
        /// <returns>Sequence of cues</returns>
        List<VisualCueType> GenerateVisualCueSequence(int count, int switchPercentage);
    }
}
