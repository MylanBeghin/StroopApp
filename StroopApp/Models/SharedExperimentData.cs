using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;

namespace StroopApp.Models
{
    public enum ExperimentAction
    {
        None,
        RestartBlock,
        NewExperiment,
        Quit
    }

    public class SharedExperimentData
    {
        public ObservableCollection<Block> Blocks { get; }
        public ObservableCollection<ISeries> BlockSeries { get; }
        public ObservableCollection<ISeries>? ColumnSerie { get; set; }
        public ObservableCollection<ReactionTimePoint> ReactionPoints { get; set; }
        public ObservableCollection<RectangularSection> Sections { get; set; }

        public Block? CurrentBlock { get; set; }
        public StroopTrial? CurrentTrial { get; set; }

        public int CurrentBlockStart { get; set; }
        public int CurrentBlockEnd { get; set; }
        public bool IsBlockFinished { get; set; } = false;
        public bool IsTaskStopped { get; set; } = false;
        public bool IsParticipantSelectionEnabled { get; set; } = true;
        public bool HasUnsavedExports { get; set; } = true;
        public ExperimentAction NextAction { get; set; } = ExperimentAction.None;

        public int ColorIndex { get; set; }

        public SharedExperimentData()
        {
            Blocks = new ObservableCollection<Block>();
            BlockSeries = new ObservableCollection<ISeries>();
            Sections = new ObservableCollection<RectangularSection>();
            ReactionPoints = new ObservableCollection<ReactionTimePoint>();
            ColumnSerie = new ObservableCollection<ISeries>();
            CurrentBlockStart = 1;
        }

        public virtual void Reset()
        {
            Blocks.Clear();
            BlockSeries.Clear();
            Sections.Clear();
            ReactionPoints.Clear();
            ColumnSerie?.Clear();

            IsTaskStopped = false;
            ColorIndex = 0;
            CurrentBlockStart = 1;
            CurrentBlockEnd = 0;
            CurrentTrial = null;
            CurrentBlock = null;
            IsBlockFinished = false;
            HasUnsavedExports = true;
            NextAction = ExperimentAction.None;
        }
    }
}