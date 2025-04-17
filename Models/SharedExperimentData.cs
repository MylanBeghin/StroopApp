using StroopApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveChartsCore;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.Kernel;

namespace StroopApp.Models
{
    public class SharedExperimentData : INotifyPropertyChanged
    {
        public ObservableCollection<StroopTrial> TrialRecords { get; }
        public ObservableCollection<ReactionTimePoint> ReactionPoints { get; set; }
        public ObservableCollection<double?> ReactionTimes { get; set; }
        public ObservableCollection<ISeries> ColumnSerie { get; set; }
        public ObservableCollection<ISeries> GlobalSerie { get; set; }

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
                    if (_currentTrial != null)
                        _currentTrial.PropertyChanged += CurrentTrial_PropertyChanged;
                }
            }
        }
        private void CurrentTrial_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StroopTrial.TrialNumber))
            {
                OnPropertyChanged(nameof(CurrentTrial));

            }
        }
        public int TotalTrials { get; set; }
        private bool _isExperimentFinished = false;
        public bool IsExperimentFinished
        {
            get => _isExperimentFinished;
            set
            {
                if (_isExperimentFinished != value)
                {
                    _isExperimentFinished = value;
                    OnPropertyChanged();
                }
            }
        }
        public SharedExperimentData(ExperimentSettings settings)
        {
            TrialRecords = new ObservableCollection<StroopTrial>();
            TotalTrials = settings.CurrentProfile.WordCount;
            ReactionPoints = new ObservableCollection<ReactionTimePoint>();
            ReactionTimes = new ObservableCollection<double?>();
            ColumnSerie = new ObservableCollection<ISeries>
            {
                new ColumnSeries<ReactionTimePoint>
                {
                    Values = ReactionPoints,
                    DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
                    DataLabelsSize = 16,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                    DataLabelsFormatter = point =>
                point.Coordinate.PrimaryValue.Equals(null)
                    ? "Aucune réponse"
                    : point.Coordinate.PrimaryValue.ToString("N0"),
                    Mapping = (point, index) => new Coordinate(
                        point.TrialNumber-1,
                        point.ReactionTime != null
                            ? point.ReactionTime.Value
                            : double.NaN
                    )
                }.OnPointCreated(p =>
    {
        if (p.Visual is null) return;
        var model = p.Model;
        if (model.IsValidResponse.HasValue)
        {
            if(model.IsValidResponse.Value)
            {
            // Bonne réponse : point vert
            p.Visual.Fill = new SolidColorPaint(SKColors.Green);
            p.Visual.Stroke = new SolidColorPaint(SKColors.Green);
            }
            else
            {
            // Mauvaise réponse : point rouge
            p.Visual.Fill = new SolidColorPaint(SKColors.Red);
            p.Visual.Stroke = new SolidColorPaint(SKColors.Red);
            }
        }
            })
            };
            GlobalSerie = new ObservableCollection<ISeries>
            {
                new LineSeries<double?>
                {
                    Values = ReactionTimes,
                    GeometrySize = 0,
                    GeometryFill = new SolidColorPaint(SKColors.CornflowerBlue)
                }

            };

        }

        public void AddTrialRecord(StroopTrial record)
        {
            TrialRecords.Add(record);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
