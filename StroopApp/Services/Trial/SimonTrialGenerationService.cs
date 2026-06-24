using StroopApp.Models;
using StroopApp.ViewModels.State;

namespace StroopApp.Services.Trial
{
    public class SimonTrialGenerationService : ITrialGenerationService
    {
        private readonly Random _random = new Random();
        public List<ITrial> GenerateTrials(ExperimentSettingsViewModel settings)
        {
            throw new NotImplementedException();
    }
    }
}
