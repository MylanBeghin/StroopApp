using StroopApp.Core;

namespace StroopApp.Models
{
    /// <summary>
    /// Represents the sex assigned at birth.
    /// </summary>
    public enum SexAssignedAtBirth
    {
        Male,
        Female,
        Intersex,
        PreferNotToAnswer
    }

    /// <summary>
    /// Represents the gender identity.
    /// </summary>
    public enum Gender
    {
        Man,
        Woman,
        NonBinary,
        Other,
        PreferNotToAnswer
    }
    /// <summary>
    /// Contains information about a participant, including demographics and experiment results.
    /// </summary>
    public class Participant : ModelBase
    {
        private string? _id;
        public string? Id
        {
            get => _id;
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }
        private double? _height;
        public double? Height
        {
            get => _height;
            set
            {
                if (value != _height)
                {
                    _height = value;
                    OnPropertyChanged();
                }
            }
        }
        private double? _weight;
        public double? Weight
        {
            get => _weight;
            set
            {
                if (value != _weight)
                {
                    _weight = value;
                    OnPropertyChanged();
                }
            }
        }
        private int? _age;
        public int? Age
        {
            get => _age;
            set
            {
                if (value != _age)
                {
                    _age = value;
                    OnPropertyChanged();
                }
            }
        }

        private SexAssignedAtBirth _sexAssigned;
        public SexAssignedAtBirth SexAssigned
        {
            get => _sexAssigned;
            set
            {
                if (value != _sexAssigned)
                {
                    _sexAssigned = value;
                    OnPropertyChanged();
                }
            }
        }

        private Gender _gender;
        public Gender Gender
        {
            get => _gender;
            set
            {
                if (value != _gender)
                {
                    _gender = value;
                    OnPropertyChanged();
                }
            }
        }
        public Participant()
        {
            SexAssigned = SexAssignedAtBirth.PreferNotToAnswer;
            Gender = Gender.PreferNotToAnswer;
        }
        /// <summary>
        /// Copies demographic properties from another participant instance.
        /// </summary>
        public void CopyPropertiesFrom(Participant src)
        {
            Height = src.Height;
            Weight = src.Weight;
            Age = src.Age;
            SexAssigned = src.SexAssigned;
            Gender = src.Gender;
        }
    }
}
