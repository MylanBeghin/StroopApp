using StroopApp.Core;
using StroopApp.Models;
using System.Collections.ObjectModel;

public class Block : ModelBase
{
    private int _blockNumber;
    public int BlockNumber
    {
        get => _blockNumber;
        set { _blockNumber = value; OnPropertyChanged(); }
    }

    private int _totalTrials;
    public int TotalTrials
    {
        get => _totalTrials;
        set { _totalTrials = value; OnPropertyChanged(); }
    }

    private double _accuracy;
    public double Accuracy
    {
        get => _accuracy;
        set { _accuracy = value; OnPropertyChanged(); }
    }

    private double? _responseTimeMean;
    public double? ResponseTimeMean
    {
        get => _responseTimeMean;
        set { _responseTimeMean = value; OnPropertyChanged(); }
    }
    private readonly ExperimentSettings _settings;

    public readonly string _profileName;
    private readonly string _stroopType;
    public string StroopType => _stroopType;
    public ObservableCollection<StroopTrial> TrialRecords { get; } = new();
    public ObservableCollection<double?> TrialTimes { get; } = new();
    public Block(ExperimentSettings settings)
    {
        _settings = settings;
        _stroopType = settings.CurrentProfile.StroopType;
        BlockNumber = settings.Block;
        _profileName = settings.CurrentProfile.ProfileName;
    }

    public void CalculateValues()
    {
        TotalTrials = TrialRecords.Count;
        Accuracy = TrialRecords.Any()
                      ? TrialRecords.Count(t => t.IsValidResponse) / (double)TotalTrials * 100
                      : 0;
        ResponseTimeMean = TrialRecords
                                    .Where(trial => trial.ReactionTime.HasValue && trial.Block == BlockNumber)
                                    .Select(trial => trial.ReactionTime)
                                    .Average();
    }
}
