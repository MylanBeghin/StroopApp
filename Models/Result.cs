using StroopApp.Core;
using System;
using System.ComponentModel;

namespace StroopApp.Models
{
    /// <summary>
    /// Represents the type of visual cue ("Amorce") used in a Stroop trial: None, Round, or Square.
    /// </summary>
    public enum AmorceType
    {
        None,
        Round,
        Square
    }
    /// <summary>
    /// Represents a single reaction time data point for a given trial, including validity of the response.
    /// </summary>
    public class Result : ModelBase
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

        public bool IsCorrect => string.Equals(ExpectedResponse, GivenResponse, StringComparison.OrdinalIgnoreCase);

        private int _reactionTime;
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
    }
}
