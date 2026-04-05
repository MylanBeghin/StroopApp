using StroopApp.ViewModels.State;
using System.Collections.ObjectModel;
namespace StroopApp.Models
{
    /// <summary>
    /// Represents a block of trials in a Stroop experiment, containing trial records and calculated statistics.
    /// </summary>
    public class Block
    {
        public int BlockNumber { get; set; }
        public int TrialsPerBlock { get; set; }
        public double Accuracy { get; set; }
        public double? ResponseTimeMean { get; set; }
        public int? CongruencePercent { get; set; }
        public int? SwitchPercent { get; set; }
        public string? BlockExperimentProfile { get; set; }
        public string? VisualCue { get; set; }
        public ObservableCollection<StroopTrial?> TrialRecords { get; } = new();
        public ObservableCollection<double?> TrialTimes { get; } = new();

        /// <summary>
        /// Legacy constructor for backward compatibility.
        /// </summary>
        public Block(string profileName, int blockNumber, int? congruencePercent, int? switchPercent, bool hasVisualCue)
        {
            BlockExperimentProfile = profileName;
            BlockNumber = blockNumber;
            CongruencePercent = congruencePercent;
            SwitchPercent = switchPercent;
            VisualCue = hasVisualCue ? "✅" : "❎";
        }

        /// <summary>
        /// Calculates block statistics including trial count, accuracy percentage, and mean response time.
        /// </summary>
        public void CalculateValues()
        {
            TrialsPerBlock = TrialRecords.Count;
            Accuracy = TrialsPerBlock > 0
            ? TrialRecords.Count(t => t.IsValidResponse == true) / (double)TrialsPerBlock * 100
            : 0;
            ResponseTimeMean = TrialRecords
                                        .Where(trial => trial.ReactionTime.HasValue && trial.Block == BlockNumber)
                                        .Select(trial => trial.ReactionTime)
                                        .Average();
        }
    }
}
