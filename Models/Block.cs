// Models/Block.cs
using StroopApp.Core;
using StroopApp.Models;

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

    private readonly string _stroopType;
    public string StroopType => _stroopType;

    private readonly IEnumerable<StroopTrial> _allTrials;

    public Block(ExperimentSettings settings)
    {
        _allTrials = settings.ExperimentContext.TrialRecords;
        _stroopType = settings.CurrentProfile.StroopType;
        BlockNumber = settings.Block;
        var trials = _allTrials.Where(t => t.Block == BlockNumber).ToList();
        TotalTrials = trials.Count;
        Accuracy = trials.Any()
                      ? trials.Count(t => t.IsValidResponse) / (double)TotalTrials * 100
                      : 0;
        ResponseTimeMean = trials
                                    .Where(trial => trial.ReactionTime.HasValue && trial.Block == BlockNumber)
                                    .Select(trial => trial.ReactionTime)
                                    .Average();
    }
}
