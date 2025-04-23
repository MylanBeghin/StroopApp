using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using LiveChartsCore;
using StroopApp.Models;
using System.ComponentModel;

namespace StroopApp.ViewModels.Experiment.Experimenter
{
    public class ExperimentGraphViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<ISeries> ColumnSerie { get; set; }
        public ObservableCollection<ISeries> GlobalSerie { get; set; }
        public ObservableCollection<ReactionTimePoint> ReactionPoints { get; set; }
        public int WordCount { get; set; }
        public int MaxReactionTime { get; set; }
        public int GroupSize { get; set; }
        public string GroupAverageLabel { get; private set; }
        public string GroupAverageValue { get; private set; }
        public ExperimentGraphViewModel(ExperimentSettings settings)
        {
            ReactionPoints = settings.ExperimentContext.ReactionPoints;
            ColumnSerie = settings.ExperimentContext.ColumnSerie;
            GlobalSerie = settings.ExperimentContext.GlobalSerie;
            WordCount = settings.CurrentProfile.WordCount;
            MaxReactionTime = settings.CurrentProfile.MaxReactionTime;
            GroupSize = settings.CurrentProfile.GroupSize;
            UpdateGroupAverage();
            ReactionPoints.CollectionChanged += (_, __) => UpdateGroupAverage();
        }
        void UpdateGroupAverage()
        {
            int total = ReactionPoints.Count;
                int idx = (total - 1) / GroupSize;
                int start = idx * GroupSize + 1;
                int end = (idx + 1) * GroupSize;
                var bloc = ReactionPoints
                    .Skip(start - 1).Take(end - start + 1)
                    .Where(p => p.ReactionTime.HasValue && !double.IsNaN(p.ReactionTime.Value))
                    .Select(p => p.ReactionTime.Value);
                GroupAverageLabel = $"Moyenne pour les mots {start} – {end} :";
                GroupAverageValue = bloc.Any()
                    ? $"{bloc.Average():N0} ms"
                    : "Aucune donnée valide";
            OnPropertyChanged(nameof(GroupAverageLabel));
            OnPropertyChanged(nameof(GroupAverageValue));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
