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
            var responseMappings = settings.KeyMappings.Simon;
            int total = profile.WordCount;
            int congruentCount = total * profile.CongruencePercent / 100;

            List<bool> congruenceFlags = GenerateCongruenceFlags(total, congruentCount, profile.StimulusPositionMode);

            var trials = new List<ITrial>();

            for (int i = 0; i < total; i++)
            {
                bool isCongruent = congruenceFlags[i];

                SimonAnswer answer = ComputeAnswer(profile.AnswerMode, _random.Next(2));
                string color = ComputeColor(answer, responseMappings);
                StimulusPosition position = ComputePosition(answer, isCongruent, profile.AnswerMode, profile.StimulusPositionMode);

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
        private StimulusPosition ComputePosition(SimonAnswer answer,bool isCongruent, SimonAnswerMode answerMode, SimonStimulusPositionMode positionMode)
        {
            if (positionMode == SimonStimulusPositionMode.Center)
                return StimulusPosition.Center;

            if(answerMode == SimonAnswerMode.GoNoGo)
                return _random.Next(2) == 0 ? StimulusPosition.Left : StimulusPosition.Right;
            
            return isCongruent
                    ? (answer == SimonAnswer.Left ? StimulusPosition.Left : StimulusPosition.Right)
                    : (answer == SimonAnswer.Left ? StimulusPosition.Right : StimulusPosition.Left);
        }
        private SimonAnswer ComputeAnswer(SimonAnswerMode answerMode, int draw)
        {
            return (answerMode, draw) switch
            {
                (SimonAnswerMode.GoNoGo, 0) => SimonAnswer.Go,
                (SimonAnswerMode.GoNoGo, 1) => SimonAnswer.NoGo,
                (SimonAnswerMode.LeftRight, 0) => SimonAnswer.Left,
                (SimonAnswerMode.LeftRight, 1) => SimonAnswer.Right,
                _ => throw new NotImplementedException(),
            };
        }
        private string ComputeColor(SimonAnswer answer, SimonResponseMappings simon)
        {
            return answer is SimonAnswer.Left or SimonAnswer.Go ? simon.Left.Color : simon.Right.Color;
        }
    }
}
