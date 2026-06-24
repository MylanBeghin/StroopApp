using StroopApp.Models.Simon;

namespace StroopApp.Models
{
    public class ExperimentKeyMappings
    {
        public KeyMappings Stroop { get; set; } = new();
        public SimonKeyMappings Simon { get; set; } = new();
    }
}
