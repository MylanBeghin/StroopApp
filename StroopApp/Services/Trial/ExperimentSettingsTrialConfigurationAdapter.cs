using StroopApp.Models;

namespace StroopApp.Services.Trial
{

    /// <summary>
    /// Adapter that projects ExperimentSettings to ITrialConfiguration.
    /// This is a temporary bridge to decouple TrialGenerationService from the god object.
    /// </summary>
    public class ExperimentSettingsTrialConfigurationAdapter : ITrialConfiguration
    {
        private readonly ExperimentSettings _settings;

        public ExperimentSettingsTrialConfigurationAdapter(ExperimentSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            if (_settings.CurrentProfile == null)
                throw new ArgumentException("CurrentProfile cannot be null", nameof(settings));

            if (_settings.Participant == null)
                throw new ArgumentException("Participant cannot be null", nameof(settings));
        }

        public int Block => _settings.Block;

        public string ParticipantId => _settings.Participant.Id;

        public int WordCount => _settings.CurrentProfile.WordCount;

        public int CongruencePercent => _settings.CurrentProfile.CongruencePercent;

        public int DominantPercent => _settings.CurrentProfile.DominantPercent;

        public bool IsAmorce => _settings.CurrentProfile.IsAmorce;

        public string TaskLanguage => _settings.CurrentProfile.TaskLanguage;
    }
}
