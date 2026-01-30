using StroopApp.Models;

using Xunit;

namespace StroopApp.XUnitTests.Models
{
    public class ParticipantTests
    {
        [Fact]
        public void Constructor_SetsDefaultValues()
        {
            // Arrange & Act
            var p = new Participant();

            // Assert
            Assert.Null(p.Id);
            Assert.Null(p.Height);
            Assert.Null(p.Weight);
            Assert.Null(p.Age);
            Assert.Equal(SexAssignedAtBirth.PreferNotToAnswer, p.SexAssigned);
            Assert.Equal(Gender.PreferNotToAnswer, p.Gender);
        }

        [Theory]
        [InlineData("abc123")]
        [InlineData("")]
        public void Id_SetValue_RaisesPropertyChanged(string newId)
        {
            // Arrange
            var p = new Participant();
            var changed = new List<string>();
            p.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            // Act
            p.Id = newId;

            // Assert
            Assert.Contains(nameof(p.Id), changed);
            Assert.Equal(newId, p.Id);
        }

        [Fact]
        public void HeightWeightAge_SetValues_RaisesPropertyChanged()
        {
            // Arrange
            var p = new Participant();
            var changed = new List<string>();
            p.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            // Act
            p.Height = 1.8;
            p.Weight = 75.0;
            p.Age = 30;

            // Assert
            Assert.Contains(nameof(p.Height), changed);
            Assert.Contains(nameof(p.Weight), changed);
            Assert.Contains(nameof(p.Age), changed);
        }

        [Fact]
        public void SexAssignedGender_SetValues_RaisesPropertyChanged()
        {
            // Arrange
            var p = new Participant();
            var changed = new List<string>();
            p.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            // Act
            p.SexAssigned = SexAssignedAtBirth.Male;
            p.Gender = Gender.NonBinary;

            // Assert
            Assert.Contains(nameof(p.SexAssigned), changed);
            Assert.Contains(nameof(p.Gender), changed);
        }

        [Fact]
        public void CopyPropertiesFrom_OverwritesAndRaisesPropertyChanged()
        {
            // Arrange
            var src = new Participant
            {
                Height = 1.7,
                Weight = 65.5,
                Age = 28,
                SexAssigned = SexAssignedAtBirth.Female,
                Gender = Gender.Woman
            };
            var dest = new Participant();
            var changed = new List<string>();
            dest.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            // Act
            dest.CopyPropertiesFrom(src);

            // Assert
            Assert.Equal(src.Height, dest.Height);
            Assert.Equal(src.Weight, dest.Weight);
            Assert.Equal(src.Age, dest.Age);
            Assert.Equal(src.SexAssigned, dest.SexAssigned);
            Assert.Equal(src.Gender, dest.Gender);

            Assert.Contains(nameof(dest.Height), changed);
            Assert.Contains(nameof(dest.Weight), changed);
            Assert.Contains(nameof(dest.Age), changed);
            Assert.Contains(nameof(dest.SexAssigned), changed);
            Assert.Contains(nameof(dest.Gender), changed);
        }
    }
}
