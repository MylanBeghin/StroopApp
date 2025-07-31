using System.Collections.ObjectModel;

using StroopApp.Core;
using StroopApp.Models;

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
	public string? _blockExperimentProfile;

	public string? BlockExperimentProfile
	{
		get => _blockExperimentProfile;
		set
		{
			_blockExperimentProfile = value;
			OnPropertyChanged();
		}
	}
	private readonly ExperimentSettings _settings;

	public ObservableCollection<StroopTrial> TrialRecords { get; } = new();
	public ObservableCollection<double?> TrialTimes { get; } = new();
	public Block(ExperimentSettings settings)
	{
		_settings = settings;
		_blockExperimentProfile = settings.CurrentProfile.ProfileName;
		_blockNumber = settings.Block;
		_congruencePercent = settings.CurrentProfile.CongruencePercent;
		_switchPercent = settings.CurrentProfile.SwitchPercent;

	}

	public void CalculateValues()
	{
		TrialsPerBlock = TrialRecords.Count;
		Accuracy = TrialRecords.Any()
					  ? TrialRecords.Count(t => t.IsValidResponse == true) / (double)TrialsPerBlock * 100
					  : 0;
		ResponseTimeMean = TrialRecords
									.Where(trial => trial.ReactionTime.HasValue && trial.Block == BlockNumber)
									.Select(trial => trial.ReactionTime)
									.Average();
	}
}
