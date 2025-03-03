using System.ComponentModel;

namespace StroopApp.Models
{
    public class ExperimentProfile
    {
        public ExperimentProfile()
        {
            _profileName = "Profil par défaut";
            _hours = 0;
            _minutes = 30;
            _seconds = 0;
            _wordDuration = 2000;
            _fixationDuration = 100;
            _amorceDuration = 0;
            _stroopType = "Congruent";
            _groupSize = 5;
            UpdateDerivedValues();
        }

        private string _profileName;

        public string ProfileName
        {
            get => _profileName;
            set
            {
                if (_profileName != value)
                {
                    _profileName = value;
                    OnPropertyChanged(nameof(ProfileName));
                }
            }
        }

        private int _hours;
        public int Hours
        {
            get => _hours;
            set
            {
                if (_hours != value)
                {
                    _hours = value;
                    OnPropertyChanged(nameof(Hours));
                    UpdateDerivedValues();
                }
            }
        }

        private int _minutes;
        public int Minutes
        {
            get => _minutes;
            set
            {
                if (_minutes != value)
                {
                    _minutes = value;
                    OnPropertyChanged(nameof(Minutes));
                    UpdateDerivedValues();
                }
            }
        }

        private int _seconds;
        public int Seconds
        {
            get => _seconds;
            set
            {
                if (_seconds != value)
                {
                    _seconds = value;
                    OnPropertyChanged(nameof(Seconds));
                    UpdateDerivedValues();
                }
            }
        }

        private int _wordDuration;
        public int WordDuration
        {
            get => _wordDuration;
            set
            {
                if (_wordDuration != value)
                {
                    _wordDuration = value;
                    OnPropertyChanged(nameof(WordDuration));
                    UpdateDerivedValues();
                }
            }
        }

        private int _fixationDuration;
        public int FixationDuration
        {
            get => _fixationDuration;
            set
            {
                if (_fixationDuration != value)
                {
                    _fixationDuration = value;
                    OnPropertyChanged(nameof(FixationDuration));
                    UpdateDerivedValues();
                }
            }
        }

        private int _amorceDuration;
        public int AmorceDuration
        {
            get => _amorceDuration;
            set
            {
                if (_amorceDuration != value)
                {
                    _amorceDuration = value;
                    OnPropertyChanged(nameof(AmorceDuration));
                    UpdateDerivedValues();
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
                    OnPropertyChanged(nameof(IsAmorce));
                    UpdateDerivedValues();
                }
            }
        }

        private string? _stroopType;
        public string? StroopType
        {
            get => _stroopType;
            set
            {
                if (_stroopType != value)
                {
                    _stroopType = value;
                    OnPropertyChanged(nameof(StroopType));
                    UpdateDerivedValues();
                }
            }
        }
        public List<string> StroopTypes { get; set; } = new List<string> { "Congruent", "Incongruent", "Amorce" };

        private int _groupSize;
        public int GroupSize
        {
            get => _groupSize;
            set
            {
                if (_groupSize != value)
                {
                    _groupSize = value;
                    OnPropertyChanged(nameof(GroupSize));
                }
            }
        }

        // Propriétés dérivées avec setter privé
        private int _taskDuration;
        public int TaskDuration
        {
            get => _taskDuration;
            private set
            {
                if (_taskDuration != value)
                {
                    _taskDuration = value;
                    OnPropertyChanged(nameof(TaskDuration));
                }
            }
        }

        private int _wordCount;
        public int WordCount
        {
            get => _wordCount;
            private set
            {
                if (_wordCount != value)
                {
                    _wordCount = value;
                    OnPropertyChanged(nameof(WordCount));
                }
            }
        }

        private int _maxReactionTime;
        public int MaxReactionTime
        {
            get => _maxReactionTime;
            private set
            {
                if (_maxReactionTime != value)
                {
                    _maxReactionTime = value;
                    OnPropertyChanged(nameof(MaxReactionTime));
                }
            }
        }

        public void UpdateDerivedValues()
        {
            IsAmorce = StroopType == "Amorce";
            // Calcul de la durée totale de la tâche en millisecondes
            TaskDuration = ((Hours * 3600) + (Minutes * 60) + Seconds) * 1000;
            if (WordDuration > 0)
            {
                if (IsAmorce)
                {
                    WordCount = TaskDuration / WordDuration;
                    MaxReactionTime = WordDuration - FixationDuration - AmorceDuration;
                }
                else
                {
                    WordCount = TaskDuration / WordDuration;
                    MaxReactionTime = WordDuration - FixationDuration;
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
