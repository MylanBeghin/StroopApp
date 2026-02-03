using StroopApp.Core;
using System.Collections.ObjectModel;
namespace StroopApp.Models
{
    /// <summary>
    /// Represents a block of trials in a Stroop experiment, containing trial records and calculated statistics.
    /// </summary>
    public class Block : ModelBase
    {
        private int _blockNumber;
        public int BlockNumber
        {
            get => _blockNumber;
            set
            {
                _blockNumber = value;
                OnPropertyChanged();
            }
        }

        private int _trialsPerBlock;
        public int TrialsPerBlock
        {
            get => _trialsPerBlock;
            set
            {
                _trialsPerBlock = value;
                OnPropertyChanged();
            }
        }

        private double _accuracy;
        public double Accuracy
        {
            get => _accuracy;
            set
            {
                _accuracy = value;
                OnPropertyChanged();
            }
        }

        private double? _responseTimeMean;
        public double? ResponseTimeMean
        {
            get => _responseTimeMean;
            set
            {
                _responseTimeMean = value;
                OnPropertyChanged();
            }
        }
        private int? _congruencePercent;

        public int? CongruencePercent
        {
            get => _congruencePercent;
            set
            {
                _congruencePercent = value;
                OnPropertyChanged();
            }
        }
        private int? _switchPercent;

        public int? SwitchPercent
        {
            get => _switchPercent;
            set
            {
                _switchPercent = value;
                OnPropertyChanged();
            }
        }
        private string? _blockExperimentProfile;

        public string? BlockExperimentProfile
        {
            get => _blockExperimentProfile;
            set
            {
                _blockExperimentProfile = value;
                OnPropertyChanged();
            }
        }
        private string? _visualCue;

        public string? VisualCue
        {
            get => _visualCue;
            set
            {
                _visualCue = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<StroopTrial> TrialRecords { get; } = new();
        public ObservableCollection<double?> TrialTimes { get; } = new();

        /// <summary>
        /// Legacy constructor for backward compatibility.
        /// </summary>
        public Block(ExperimentSettings settings)
        {
            var config = new ExperimentSettingsBlockConfigurationAdapter(settings);
            InitializeFromConfiguration(config);
        }

        /// <summary>
        /// Primary constructor accepting minimal configuration interface.
        /// </summary>
        public Block(IBlockConfiguration config)
        {
            InitializeFromConfiguration(config);
        }

        private void InitializeFromConfiguration(IBlockConfiguration config)
        {
            _blockExperimentProfile = config.ProfileName;
            _blockNumber = config.Block;
            _congruencePercent = config.CongruencePercent;
            _switchPercent = config.SwitchPercent;
            if (config.IsAmorce)
                _visualCue = "✅";
            else
                _visualCue = "❎";
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
