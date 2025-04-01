using StroopApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class SharedExperimentDataModel : INotifyPropertyChanged
{
    public ObservableCollection<StroopTrialRecord> TrialRecords { get; }

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

    public SharedExperimentDataModel(ExperimentSettings Settings)
    {
        TrialRecords = new ObservableCollection<StroopTrialRecord>();
        TotalTrials = Settings.CurrentProfile.WordCount;
        CurrentTrial = 0;
    }

    public void AddTrialRecord(StroopTrialRecord record)
    {
        TrialRecords.Add(record);
        CurrentTrial = TrialRecords.Count;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
