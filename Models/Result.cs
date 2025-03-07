using System;
using System.ComponentModel;

namespace StroopApp.Models
{
    public enum AmorceType
    {
        None,    // For non-amorce Stroop types
        Round,
        Square
    }
    public class Result : INotifyPropertyChanged
    {
        private int _participantId;
        public int ParticipantId
        {
            get => _participantId;
            set
            {
                if (value != _participantId)
                {
                    _participantId = value;
                    OnPropertyChanged(nameof(ParticipantId));
                }
            }
        }

        private string _stroopType;
        public string StroopType
        {
            get => _stroopType;
            set
            {
                if (value != _stroopType)
                {
                    _stroopType = value;
                    OnPropertyChanged(nameof(StroopType));
                }
            }
        }

        private int _block;
        /// <summary>
        /// Block number; for instance, 1 for the first block, 2 for the second, etc.
        /// </summary>
        public int Block
        {
            get => _block;
            set
            {
                if (value != _block)
                {
                    _block = value;
                    OnPropertyChanged(nameof(Block));
                }
            }
        }

        private string _expectedResponse;
        public string ExpectedResponse
        {
            get => _expectedResponse;
            set
            {
                if (value != _expectedResponse)
                {
                    _expectedResponse = value;
                    OnPropertyChanged(nameof(ExpectedResponse));
                    OnPropertyChanged(nameof(IsCorrect));
                }
            }
        }

        private string _givenResponse;
        public string GivenResponse
        {
            get => _givenResponse;
            set
            {
                if (value != _givenResponse)
                {
                    _givenResponse = value;
                    OnPropertyChanged(nameof(GivenResponse));
                    OnPropertyChanged(nameof(IsCorrect));
                }
            }
        }

        /// <summary>
        /// Returns true if the expected response equals the given response (case-insensitive).
        /// </summary>
        public bool IsCorrect => string.Equals(ExpectedResponse, GivenResponse, StringComparison.OrdinalIgnoreCase);

        private int _reactionTime;
        /// <summary>
        /// Reaction time in milliseconds.
        /// </summary>
        public int ReactionTime
        {
            get => _reactionTime;
            set
            {
                if (value != _reactionTime)
                {
                    _reactionTime = value;
                    OnPropertyChanged(nameof(ReactionTime));
                }
            }
        }

        private int _trialNumber;
        /// <summary>
        /// Trial number (i.e., the word number in the block).
        /// </summary>
        public int TrialNumber
        {
            get => _trialNumber;
            set
            {
                if (value != _trialNumber)
                {
                    _trialNumber = value;
                    OnPropertyChanged(nameof(TrialNumber));
                }
            }
        }

        private AmorceType _amorce;
        /// <summary>
        /// Cue type: Round or Square; set to None if the Stroop type is not "Amorce".
        /// </summary>
        public AmorceType Amorce
        {
            get => _amorce;
            set
            {
                if (value != _amorce)
                {
                    _amorce = value;
                    OnPropertyChanged(nameof(Amorce));
                }
            }
        }

        // Constructor
        public Result()
        {
            ParticipantId = 0;
            StroopType = string.Empty;
            Block = 1;
            ExpectedResponse = string.Empty;
            GivenResponse = string.Empty;
            ReactionTime = 0;
            TrialNumber = 0;
            Amorce = AmorceType.None;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
