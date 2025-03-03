namespace StroopApp.Models
{
    public class ExperimentSettings
    {
        public string? ParticipantName { get; set; }
        public ExperimentProfile CurrentProfile { get; set; }
        public ExperimentSettings()
        {
            CurrentProfile = new ExperimentProfile();
        }
    }

}
