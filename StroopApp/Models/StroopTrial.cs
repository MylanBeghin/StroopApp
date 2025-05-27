using StroopApp.Core;

namespace StroopApp.Models
{
	/// <summary>
	/// Represents a single trial in a Stroop task, containing stimulus data, participant responses,
	/// reaction time, and trial metadata such as block and trial number.
	/// </summary>
	public class StroopTrial : ModelBase
	{
		private string _participantId;
		public string ParticipantId
		{
			get => _participantId;
			set
			{
				if (_participantId != value)
				{
					_participantId = value;
					OnPropertyChanged();
				}
			}
		}
		private bool _isCongruent;

		public bool IsCongruent
		{
			get => _isCongruent;
			set
			{
				if (_isCongruent != value)
				{
					_isCongruent = value;
					OnPropertyChanged();
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
					OnPropertyChanged();
				}
			}
		}
		private int _switchPourcentage;
		public int SwitchPourcentage
		{
			get => _switchPourcentage;
			set
			{
				if (_switchPourcentage != value)
				{
					_switchPourcentage = value;
					OnPropertyChanged();
				}
			}
		}
		private int _congruencePourcentage;
		public int CongruencePourcentage
		{
			get => _congruencePourcentage;
			set
			{
				if (_congruencePourcentage != value)
				{
					_congruencePourcentage = value;
					OnPropertyChanged();
				}
			}
		}

		private int _block;
		public int Block
		{
			get => _block;
			set
			{
				if (_block != value)
				{
					_block = value;
					OnPropertyChanged();
				}
			}
		}
		private Word _stimulus;
		public Word Stimulus
		{
			get => _stimulus;
			set
			{
				if (_stimulus != value)
				{
					_stimulus = value;
					OnPropertyChanged();
				}
			}
		}

		private string _expectedAnswer;
		public string ExpectedAnswer
		{
			get => _expectedAnswer;
			set
			{
				if (_expectedAnswer != value)
				{
					_expectedAnswer = value;
					OnPropertyChanged();
				}
			}
		}

		private string _givenAnswer;
		public string GivenAnswer
		{
			get => _givenAnswer;
			set
			{
				if (_givenAnswer != value)
				{
					_givenAnswer = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isValidResponse;
		public bool IsValidResponse
		{
			get => _isValidResponse;
			set
			{
				if (_isValidResponse != value)
				{
					_isValidResponse = value;
					OnPropertyChanged();
				}
			}
		}

		private double? _reactionTime;
		public double? ReactionTime
		{
			get => _reactionTime;
			set
			{
				if (_reactionTime != value)
				{
					_reactionTime = value;
					OnPropertyChanged();
				}
			}
		}

		private int _trialNumber;
		public int TrialNumber
		{
			get => _trialNumber;
			set
			{
				if (_trialNumber != value)
				{
					_trialNumber = value;
					OnPropertyChanged();
				}
			}
		}
		public AmorceType Amorce
		{
			get; set;
		}

		public void DetermineExpectedAnswer()
		{
			if (IsAmorce)
			{
				ExpectedAnswer = (Amorce == AmorceType.Square) ? Stimulus.Text : Stimulus.Color;
			}
			else if (IsCongruent)
			{
				ExpectedAnswer = Stimulus.Text;
			}
			else
			{
				ExpectedAnswer = Stimulus.Color;
			}
		}
		public override string ToString()
		{
			return "Stimulus : " + Stimulus + "\nType d'amorce :" + Amorce + "\nExpected answer : " + ExpectedAnswer;
		}
	}
}
