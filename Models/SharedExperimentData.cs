using StroopApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class SharedExperimentData : INotifyPropertyChanged
{
    public ObservableCollection<StroopTrial> TrialRecords { get; }

    private int _currentTrial;
    public int CurrentTrial
    {
        get => _currentTrial;
        set
        {
            if (_currentTrial != value)
            {
                _currentTrial = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Progress));
            }
        }
    }

    public int TotalTrials { get; set; }

    public double Progress => TotalTrials > 0 ? (double)CurrentTrial / TotalTrials : 0;

    public SharedExperimentData(ExperimentSettings Settings)
    {
        TrialRecords = new ObservableCollection<StroopTrial>();
        TotalTrials = Settings.CurrentProfile.WordCount;
        CurrentTrial = 0;
    }

    public void AddTrialRecord(StroopTrial record)
    {
        TrialRecords.Add(record);
        CurrentTrial = TrialRecords.Count;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
