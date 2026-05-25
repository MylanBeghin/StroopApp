namespace StroopApp.Core.Models
{
    public class ExperimentConfiguration
    {
        public string Name { get; set; } = "New Experiment";
        public List<IExperimentStep> Steps { get; set; } = [];
    }
}
