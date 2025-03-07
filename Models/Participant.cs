using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StroopApp.Models
{
    public enum SexAssignedAtBirth
    {
        Male,
        Female,
        Intersex,
        PreferNotToAnswer
    }

    public enum Gender
    {
        Man,
        Woman,
        NonBinary,
        Other,
        PreferNotToAnswer
    }
    public class Participant : INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        // Height (e.g., in centimeters). Nullable: if the participant prefers not to answer, this can be null.
        private double? _height;
        public double? Height
        {
            get => _height;
            set
            {
                if (value != _height)
                {
                    _height = value;
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        // Weight (e.g., in kilograms)
        private double? _weight;
        public double? Weight
        {
            get => _weight;
            set
            {
                if (value != _weight)
                {
                    _weight = value;
                    OnPropertyChanged(nameof(Weight));
                }
            }
        }

        // Age (in years)
        private int? _age;
        public int? Age
        {
            get => _age;
            set
            {
                if (value != _age)
                {
                    _age = value;
                    OnPropertyChanged(nameof(Age));
                }
            }
        }

        private SexAssignedAtBirth _sexAssigned;
        /// <summary>
        /// Sex assigned at birth.
        /// </summary>
        public SexAssignedAtBirth SexAssigned
        {
            get => _sexAssigned;
            set
            {
                if (value != _sexAssigned)
                {
                    _sexAssigned = value;
                    OnPropertyChanged(nameof(SexAssigned));
                }
            }
        }

        private Gender _gender;
        /// <summary>
        /// The participant's gender.
        /// </summary>
        public Gender Gender
        {
            get => _gender;
            set
            {
                if (value != _gender)
                {
                    _gender = value;
                    OnPropertyChanged(nameof(Gender));
                }
            }
        }

        private List<Result> _results;
        /// <summary>
        /// List of results for this participant.
        /// </summary>
        public List<Result> Results
        {
            get => _results;
            set
            {
                if (value != _results)
                {
                    _results = value;
                    OnPropertyChanged(nameof(Results));
                }
            }
        }

        // Constructor
        public Participant()
        {
            Results = new List<Result>();
            Height = null;
            Weight = null;
            Age = null;
            SexAssigned = SexAssignedAtBirth.PreferNotToAnswer;
            Gender = Gender.PreferNotToAnswer;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
