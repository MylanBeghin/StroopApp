using StroopApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class SharedExperimentData : INotifyPropertyChanged
{
    public ObservableCollection<StroopTrial> TrialRecords { get; }

    private StroopTrial _currentTrial;
    public StroopTrial CurrentTrial
    {
        get => _currentTrial;
        set
        {
            if (_currentTrial != value)
            {
                if (_currentTrial != null)
                    _currentTrial.PropertyChanged -= CurrentTrial_PropertyChanged;

                _currentTrial = value;
                OnPropertyChanged(nameof(CurrentTrial));
                OnPropertyChanged(nameof(Progress));  // Notifie le changement de progress

                if (_currentTrial != null)
                    _currentTrial.PropertyChanged += CurrentTrial_PropertyChanged;
            }
        }
    }

    private void CurrentTrial_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(StroopTrial.TrialNumber))
        {
            // Lorsqu'un trial change de numéro, on met à jour Progress (et éventuellement CurrentTrial si nécessaire)
            OnPropertyChanged(nameof(Progress));
            OnPropertyChanged(nameof(CurrentTrial));
        }
    }

    public int TotalTrials { get; set; }

    // Progress retourne la progression sous forme de double entre 0 et 1.
    // Pour obtenir un int multiplié par 100, on pourra faire : (int)(Progress * 100)
    public double Progress => TotalTrials > 0 && CurrentTrial != null
        ? (double)CurrentTrial.TrialNumber / TotalTrials
        : 0;

    public SharedExperimentData(ExperimentSettings settings)
    {
        TrialRecords = new ObservableCollection<StroopTrial>();
        TotalTrials = settings.CurrentProfile.WordCount;
    }

    public void AddTrialRecord(StroopTrial record)
    {
        TrialRecords.Add(record);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
