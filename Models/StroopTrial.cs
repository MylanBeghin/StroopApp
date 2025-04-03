using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StroopApp.Models
{
    public class StroopTrial : INotifyPropertyChanged
    {
        private int _participantId;
        public int ParticipantId
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

        private string _stroopType;
        public string StroopType
        {
            get => _stroopType;
            set
            {
                if (_stroopType != value)
                {
                    _stroopType = value;
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

        private double _reactionTime;
        public double ReactionTime
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

        public AmorceType Amorce { get; set; }

        public void DetermineExpectedAnswer()
        {
            if (string.Equals(StroopType, "Amorce", StringComparison.OrdinalIgnoreCase))
            {
                ExpectedAnswer = (Amorce == AmorceType.Square) ? Stimulus.Text : Stimulus.Color;
            }
            else if (string.Equals(StroopType, "Congruent", StringComparison.OrdinalIgnoreCase))
            {
                ExpectedAnswer = Stimulus.Text;
            }
            else if (string.Equals(StroopType, "Incongruent", StringComparison.OrdinalIgnoreCase))
            {
                ExpectedAnswer = Stimulus.Color;
            }
        }
        public override string ToString()
        {
            return "Stimulus : " + Stimulus + "\nType d'amorce :" + Amorce + "\nExpected answer : " + ExpectedAnswer;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
