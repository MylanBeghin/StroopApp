using StroopApp.Models;
using StroopApp.Models.Simon;
using StroopApp.ViewModels.State;

namespace StroopApp.Services.Trial
{
    public class SimonTrialGenerationService : ISimonTrialGenerationService
    {
        private readonly Random _random = new Random();

        public SimonTrialGenerationService() { }
        public List<ITrial> GenerateTrials(ExperimentSettingsViewModel settings)
        {
            if (settings?.CurrentProfile is null)
                throw new ArgumentException("CurrentProfile cannot be null", nameof(settings));
            if (settings?.Participant is null)
                throw new ArgumentException("Participant cannot be null", nameof(settings));

            SimonProfile profile = (SimonProfile)settings.CurrentProfile;
            var simon = settings.KeyMappings.Simon;
            int total = profile.WordCount;
            int congruentCount = total * profile.CongruencePercent / 100;

            List<bool> congruenceFlags = GenerateCongruenceFlags(total, congruentCount, profile.StimulusPositionMode);

            var trials = new List<ITrial>();

            for (int i = 0; i < total; i++)
            {
                bool isCongruent = congruenceFlags[i];

                SimonAnswer answer = _random.Next(2) == 0 ? SimonAnswer.Left : SimonAnswer.Right;

                string color = answer == SimonAnswer.Left ? simon.Left.Color : simon.Right.Color;

                StimulusPosition position = ComputePosition(answer, isCongruent, profile.StimulusPositionMode);

                trials.Add(new SimonTrial
                {
                    TrialNumber = i + 1,
                    Block = settings.Block,
                    ParticipantId = settings.Participant.Id,
                    CongruencePercent = profile.CongruencePercent,
                    IsCongruent = isCongruent,
                    ExpectedAnswer = answer,
                    Stimulus = new SimonStimulus(color, position, string.Empty)
                });

            }
            return trials;
        }

        private List<bool> GenerateCongruenceFlags(int totalStimulusCount,int congruentCount, SimonStimulusPositionMode mode )
        {
            return mode == SimonStimulusPositionMode.Center ?
                [.. Enumerable.Repeat(true, totalStimulusCount)] :
                Enumerable.Repeat(true, congruentCount)
                .Concat(
                    Enumerable.Repeat(false, totalStimulusCount - congruentCount))
                .OrderBy(_ => _random.Next())
                .ToList();
        }
        private StimulusPosition ComputePosition(SimonAnswer answer,bool isCongruent, SimonStimulusPositionMode mode)
        {
            return mode == SimonStimulusPositionMode.Center ?
                    StimulusPosition.Center : isCongruent
                    ? (answer == SimonAnswer.Left ? StimulusPosition.Left : StimulusPosition.Right)
                    : (answer == SimonAnswer.Left ? StimulusPosition.Right : StimulusPosition.Left);
        }
    }
}
