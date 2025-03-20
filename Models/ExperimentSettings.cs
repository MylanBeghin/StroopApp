namespace StroopApp.Models
{
    public class ExperimentSettings
    {
        public ParticipantModel Participant { get; set; }
        public ExperimentProfile CurrentProfile { get; set; }
        public KeyMappings KeyMappings { get; set; }
        public ExperimentSettings()
        {
            CurrentProfile = new ExperimentProfile();
            KeyMappings = new KeyMappings();
        }
    }

}
