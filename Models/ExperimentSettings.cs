using StroopApp.Core;
namespace StroopApp.Models
{
    /// <summary>
    /// Holds the current configuration state for an experiment, including the selected participant,
    /// preset profile, key mappings, shared context, and current block index.
    /// </summary>

    public class ExperimentSettings : ModelBase
    {
        private int block;

        public int Block
        {
            get => block;
            set
            {
                if (value != block)
                {
                    block = value;
                    OnPropertyChanged();
                }

            }
        }
        private Participant _participant;
        public Participant Participant
        {
            get => _participant;
            set
            {
                _participant = value;
                OnPropertyChanged();
            }
        }

        private ExperimentProfile _currentProfile;
        public ExperimentProfile CurrentProfile
        {
            get => _currentProfile;
            set
            {
                _currentProfile = value;
                OnPropertyChanged();
            }
        }

        private KeyMappings _keyMappings;
        public KeyMappings KeyMappings
        {
            get => _keyMappings;
            set
            {
                _keyMappings = value;
                OnPropertyChanged();
            }
        }
        private SharedExperimentData _experimentContext;
        public SharedExperimentData ExperimentContext
        {
            get => _experimentContext;
            set
            {
                _experimentContext = value;
                OnPropertyChanged();
            }
        }
        private string _exportFolderPath;

        public string ExportFolderPath
        {
            get => _exportFolderPath;
            set
            {
                if (_exportFolderPath != value)
                {
                    _exportFolderPath = value;
                    OnPropertyChanged(nameof(ExportFolderPath));
                }
            }
        }
        public void Reset()
        {
            ExperimentContext.Reset();
            Block = 1;
            OnPropertyChanged(string.Empty);
        }
        public ExperimentSettings()
        {
            CurrentProfile = new ExperimentProfile();
            KeyMappings = new KeyMappings();
            ExperimentContext = new SharedExperimentData();
            ExportFolderPath = "";
            Block = 1;
        }
    }
}
