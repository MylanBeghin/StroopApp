﻿using StroopApp.Core;

namespace StroopApp.Models
{
	/// /// <summary>
	/// Defines the calculation mode used to determine experiment length:
	/// either by total task duration or by word count.
	/// </summary>

	public enum CalculationMode
	{
		TaskDuration,
		WordCount
	}
	/// <summary>
	/// Represents a saved set of experiment parameters, including durations, timings, group size, and calculation mode.
	/// Automatically updates derived values based on the selected <see cref="CalculationMode"/>.
	/// </summary>
	public class ExperimentProfile : ModelBase
	{
		public ExperimentProfile()
		{
			_id = Guid.NewGuid();
			_profileName = "Nouveau Profil";
			_fixationDuration = 100;
			_maxReactionTime = 400;
			_amorceDuration = 0;
			_groupSize = 5;
			_isAmorce = false;
			_wordCount = 10;
			_calculationMode = CalculationMode.WordCount;
			_congruencePercent = 50;
			_dominantPercent = 50;
			_switchPercent = null;
			UpdateDerivedValues();
		}
		private Guid _id;
		public Guid Id
		{
			get => _id;
			set => _id = value;
		}
		private string _profileName;

		public string ProfileName
		{
			get => _profileName;
			set
			{
				if (_profileName != value)
				{
					_profileName = value;
					OnPropertyChanged();
				}
			}
		}

		private int _hours;
		public int Hours
		{
			get => _hours;
			set
			{
				if (_hours != value)
				{
					_hours = value;
					OnPropertyChanged();
					UpdateDerivedValues();
				}
			}
		}

		private int _minutes;
		public int Minutes
		{
			get => _minutes;
			set
			{
				if (_minutes != value)
				{
					_minutes = value;
					OnPropertyChanged();
					UpdateDerivedValues();
				}
			}
		}

		private int _seconds;
		public int Seconds
		{
			get => _seconds;
			set
			{
				if (_seconds != value)
				{
					_seconds = value;
					OnPropertyChanged();
					UpdateDerivedValues();
				}
			}
		}

		private int _wordDuration;
		public int WordDuration
		{
			get => _wordDuration;
			set
			{
				if (_wordDuration != value)
				{
					_wordDuration = value;
					OnPropertyChanged();
					UpdateDerivedValues();
				}
			}
		}

		private int _fixationDuration;
		public int FixationDuration
		{
			get => _fixationDuration;
			set
			{
				if (_fixationDuration != value)
				{
					_fixationDuration = value;
					OnPropertyChanged();
					UpdateDerivedValues();
				}
			}
		}

		private int _amorceDuration;
		public int AmorceDuration
		{
			get => _amorceDuration;
			set
			{
				if (_amorceDuration != value)
				{
					_amorceDuration = value;
					OnPropertyChanged();
					UpdateDerivedValues();
				}
			}
		}

		private bool _isAmorce;
		public bool IsAmorce
		{
			get => _isAmorce;
			set
			{
				if (_isAmorce != value)
				{
					_isAmorce = value;
					if (!_isAmorce)
					{
						AmorceDuration = 0;
						_dominantPercent = 50;
						_switchPercent = null;
						OnPropertyChanged(nameof(DominantPercent));
						OnPropertyChanged(nameof(SwitchPercent));
					}
					else
					{
						_switchPercent = 50;
						OnPropertyChanged(nameof(SwitchPercent));
					}
					OnPropertyChanged();
					UpdateDerivedValues();
				}
			}
		}

		private int _groupSize;
		public int GroupSize
		{
			get => _groupSize;
			set
			{
				if (_groupSize != value)
				{
					_groupSize = value;
					OnPropertyChanged();
				}
			}
		}
		private int _taskDuration;
		public int TaskDuration
		{
			get => _taskDuration;
			set
			{
				if (_taskDuration != value)
				{
					_taskDuration = value;
					OnPropertyChanged();
				}
			}
		}

		private int _wordCount;
		public int WordCount
		{
			get => _wordCount;
			set
			{
				if (_wordCount != value)
				{
					_wordCount = value;
					OnPropertyChanged();
				}
			}
		}

		private int _maxReactionTime;
		public int MaxReactionTime
		{
			get => _maxReactionTime;
			set
			{
				if (_maxReactionTime != value)
				{
					_maxReactionTime = value;
					OnPropertyChanged();
					UpdateDerivedValues();
				}
			}
		}
		private CalculationMode _calculationMode;
		public CalculationMode CalculationMode
		{
			get => _calculationMode;
			set
			{
				if (_calculationMode != value)
				{
					_calculationMode = value;
					OnPropertyChanged();
					UpdateDerivedValues();
				}
			}
		}
		private int _dominantPercent;
		public int DominantPercent
		{
			get => _dominantPercent;
			set
			{
				if (_dominantPercent != value)
				{
					_dominantPercent = value;
					OnPropertyChanged();
				}
			}
		}
		private int _congruencePercent;
		public int CongruencePercent
		{
			get => _congruencePercent;
			set
			{
				if (_congruencePercent != value)
				{
					_congruencePercent = value;

					OnPropertyChanged();
				}
			}
		}
		private int? _switchPercent;
		public int? SwitchPercent
		{
			get => _switchPercent;
			set
			{
				if (_switchPercent != value)
				{
					_switchPercent = value;
					OnPropertyChanged();
				}
			}
		}
		public void UpdateDerivedValues()
		{
			WordDuration = MaxReactionTime + FixationDuration + AmorceDuration;
			if (CalculationMode == CalculationMode.TaskDuration)
			{
				TaskDuration = ((Hours * 3600) + (Minutes * 60) + Seconds) * 1000;
				if (WordDuration > 0)
				{
					WordCount = TaskDuration / WordDuration;
				}
			}
			else if (CalculationMode == CalculationMode.WordCount)
			{
				TaskDuration = WordCount * WordDuration;
				Hours = TaskDuration / 3600000;
				Minutes = (TaskDuration % 3600000) / 60000;
				Seconds = (TaskDuration % 60000) / 1000;
			}
		}
		public ExperimentProfile CloneProfile()
		{
			return new ExperimentProfile
			{
				Id = this.Id,
				ProfileName = this.ProfileName,
				CalculationMode = this.CalculationMode,
				Hours = this.Hours,
				Minutes = this.Minutes,
				Seconds = this.Seconds,
				TaskDuration = this.TaskDuration,
				WordDuration = this.WordDuration,
				MaxReactionTime = this.MaxReactionTime,
				GroupSize = this.GroupSize,
				AmorceDuration = this.AmorceDuration,
				FixationDuration = this.FixationDuration,
				WordCount = this.WordCount,
				IsAmorce = this.IsAmorce,
				DominantPercent = this.DominantPercent,
				CongruencePercent = this.CongruencePercent,
				SwitchPercent = this.SwitchPercent
			};
		}
	}
}
