using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StroopApp.Models
{
    public class ExperimentSettings : INotifyPropertyChanged
    {
        private ParticipantModel _participant;
        public ParticipantModel Participant
        {
            get => _participant;
            set { _participant = value; OnPropertyChanged(); }
        }

        private ExperimentProfile _currentProfile;
        public ExperimentProfile CurrentProfile
        {
            get => _currentProfile;
            set { _currentProfile = value; OnPropertyChanged(); }
        }

        private KeyMappings _keyMappings;
        public KeyMappings KeyMappings
        {
            get => _keyMappings;
            set { _keyMappings = value; OnPropertyChanged(); }
        }

        public ExperimentSettings()
        {
            CurrentProfile = new ExperimentProfile();
            KeyMappings = new KeyMappings();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
