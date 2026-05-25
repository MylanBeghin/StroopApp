using CommunityToolkit.Mvvm.ComponentModel;
using StroopApp.Core.Models;
using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.ViewModels.State;
using StroopApp.Views.Experiment.Participant;
using CoreConfig = StroopApp.Core.Models.ExperimentConfiguration;

namespace StroopApp.Core.Services
{
    /// <summary>
    /// Centralized service that drives the linear execution of an experiment sequence.
    /// Tracks the current participant, current step, and running state.
    /// Uses IParticipantNavigationService to push the correct page into the Participant Window.
    /// </summary>
    public partial class SequenceManager : ObservableObject, ISequenceManager
    {
        private readonly IParticipantNavigationService _participantNav;

        // ExperimentSettingsViewModel is temporary for the existing Stroop flow.
        private readonly ExperimentSettingsViewModel _legacySettings;

        private List<IExperimentStep> _steps = [];
        private int _currentIndex = -1;

        [ObservableProperty]
        private Participant? _currentParticipant;

        [ObservableProperty]
        private IExperimentStep? _currentStep;

        [ObservableProperty]
        private bool _isRunning;

        public SequenceManager(
            IParticipantNavigationService participantNav,
            ExperimentSettingsViewModel legacySettings)
        {
            _participantNav = participantNav;
            _legacySettings = legacySettings;
        }

        /// <inheritdoc/>
        public void StartExperiment(CoreConfig config, Participant participant)
        {
            if (IsRunning)
                throw new InvalidOperationException("An experiment is already running. Call CancelExperiment() first.");

            _steps = config.Steps ?? throw new ArgumentNullException(nameof(config.Steps));
            _currentIndex = -1;
            CurrentParticipant = participant;
            IsRunning = true;

            MoveToNextStep();
        }

        /// <inheritdoc/>
        public void MoveToNextStep()
        {
            if (!IsRunning)
                return;

            _currentIndex++;

            if (_currentIndex >= _steps.Count)
            {
                EndExperiment();
                return;
            }

            CurrentStep = _steps[_currentIndex];
            ExecuteCurrentStep();
        }

        /// <inheritdoc/>
        public void CancelExperiment()
        {
            if (!IsRunning)
                return;

            IsRunning = false;
            CurrentStep = null;
            CurrentParticipant = null;
            _steps = [];
            _currentIndex = -1;
        }

        private void ExecuteCurrentStep()
        {
            switch (CurrentStep)
            {
                case InstructionStep instruction:
                    throw new NotSupportedException(
                        $"InstructionStep '{instruction.Name}' navigation is not yet implemented. Implement in Phase 3.");

                case TaskStep { TaskTypeIdentifier: "Stroop" } taskStep:
                    _participantNav.NavigateTo(() =>
                        new StroopPage(_participantNav, _legacySettings));
                    break;

                case TaskStep { TaskTypeIdentifier: "Fake" } taskStep:
                    throw new NotSupportedException(
                        $"FakeTask '{taskStep.Name}' navigation is not yet implemented. Implement in Phase 3.");

                default:
                    throw new NotSupportedException(
                        $"Unknown step type '{CurrentStep?.GetType().Name}'. Register a handler in ExecuteCurrentStep().");
            }
        }

        private void EndExperiment()
        {
            IsRunning = false;
            CurrentStep = null;
            CurrentParticipant = null;
            _steps = [];
            _currentIndex = -1;
        }
    }
}
