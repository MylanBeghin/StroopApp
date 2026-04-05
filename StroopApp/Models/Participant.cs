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
    public class Participant
    {
        public string? Id { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public int? Age { get; set; }

        public SexAssignedAtBirth SexAssigned { get; set; }
        public Gender Gender { get; set; }
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
