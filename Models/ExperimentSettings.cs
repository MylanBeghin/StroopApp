using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using StroopApp.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Events;
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
        private SharedExperimentData _experimentContext;
        public SharedExperimentData ExperimentContext
        {
            get => _experimentContext;
            set { _experimentContext = value; OnPropertyChanged(); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public ExperimentSettings()
        {
            CurrentProfile = new ExperimentProfile();
            KeyMappings = new KeyMappings();
            ExperimentContext = new SharedExperimentData(this);
            Block = 1;
        }
        public void NewBlock()
        {
            Block++;
            ExperimentContext.IsBlockFinished = false;
            ExperimentContext.ReactionPoints = new ObservableCollection<ReactionTimePoint>();
            ExperimentContext.ColumnSerie = new ObservableCollection<ISeries>
            {
                new ColumnSeries<ReactionTimePoint>
                {
                    Values = ExperimentContext.ReactionPoints,
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

        }
    }
}
