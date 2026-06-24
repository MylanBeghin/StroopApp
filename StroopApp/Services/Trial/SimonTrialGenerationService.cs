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

            var simon = settings.KeyMappings.Simon;
            int total = settings.CurrentProfile.WordCount;
            int congruentCount = total * settings.CurrentProfile.CongruencePercent / 100;

            List<bool> congruenceFlags =
                Enumerable.Repeat(true, congruentCount)
                .Concat(
                    Enumerable.Repeat(false, total - congruentCount))
                .OrderBy(_ => _random.Next())
                .ToList();

            var trials = new List<ITrial>();

            for (int i = 0; i < total; i++)
            {
                bool isCongruent = congruenceFlags[i];

                SimonAnswer answer = _random.Next(2) == 0
                    ? SimonAnswer.Left
                    : SimonAnswer.Right;

                string color = answer == SimonAnswer.Left
                    ? simon.Left.Color
                    : simon.Right.Color;

                StimulusPosition position = isCongruent
                    ? (answer == SimonAnswer.Left ? StimulusPosition.Left : StimulusPosition.Right)
                    : (answer == SimonAnswer.Left ? StimulusPosition.Right : StimulusPosition.Left);


                trials.Add(new SimonTrial
                {
                    TrialNumber = i + 1,
                    Block = settings.Block,
                    ParticipantId = settings.Participant.Id,
                    CongruencePercent = settings.CurrentProfile.CongruencePercent,
                    IsCongruent = isCongruent,
                    ExpectedAnswer = answer,
                    Stimulus = new SimonStimulus(color, position, string.Empty)
                });

            }
            return trials;
        }
    }
}
