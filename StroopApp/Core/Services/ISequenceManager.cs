using StroopApp.Core.Models;
using StroopApp.Models;
using CoreConfig = StroopApp.Core.Models.ExperimentConfiguration;

namespace StroopApp.Core.Services
{
    /// <summary>
    /// Controls the linear execution of an experiment sequence.
    /// Drives the Participant Window navigation step by step.
    /// </summary>
    public interface ISequenceManager
    {
        /// <summary>The participant currently running the experiment. Null when idle.</summary>
        Participant? CurrentParticipant { get; }

        /// <summary>The step currently being executed. Null when idle or finished.</summary>
        IExperimentStep? CurrentStep { get; }

        /// <summary>True while an experiment sequence is actively running.</summary>
        bool IsRunning { get; }

        /// <summary>
        /// Loads the given configuration, registers the participant, and executes the first step.
        /// </summary>
        void StartExperiment(CoreConfig config, Participant participant);

        /// <summary>
        /// Advances to the next step. Calls EndExperiment() automatically when all steps are done.
        /// </summary>
        void MoveToNextStep();

        /// <summary>
        /// Immediately halts the running experiment without completing it.
        /// </summary>
        void CancelExperiment();
    }
}
