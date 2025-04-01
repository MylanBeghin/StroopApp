using System.ComponentModel;
using System.Runtime.CompilerServices;

public enum StroopType
{
    Congruent,
    Incongruent,
    Amorce
}

public enum AmorceType
{
    Round,
    Square
}

public class StroopTrialRecord : INotifyPropertyChanged
{
    private string _participantId;
    public string ParticipantId
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

    public StroopType StroopType { get; set; }

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

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
