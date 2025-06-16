using StroopApp.Core;
using StroopApp.Models;
using System.Collections.ObjectModel;

namespace StroopApp.ViewModels.Experiment.Experimenter
{
    internal class LiveReactionTimeViewModel : ViewModelBase
    {
        private ExperimentSettings _settings;
        public ObservableCollection<ReactionTimePoint> ReactionPoints { get; set; }

        public string GroupAverageLabel { get; private set; }
        public string GroupAverageValue { get; private set; }
        
        public LiveReactionTimeViewModel(ExperimentSettings settings)
        {
            _settings = settings;
            ReactionPoints = _settings.ExperimentContext.ReactionPoints;
            UpdateGroupAverage();
            _settings.ExperimentContext.ReactionPoints.CollectionChanged += (_, __) => UpdateGroupAverage();
        }
        void UpdateGroupAverage()
        {
            int total = ReactionPoints.Count;
            int idx = (total - 1) / _settings.CurrentProfile.GroupSize;
            int start = idx * _settings.CurrentProfile.GroupSize + 1;
            int end = (idx + 1) * _settings.CurrentProfile.GroupSize;
            var group = ReactionPoints
                .Skip(start - 1).Take(end - start + 1)
                .Where(p => p.ReactionTime.HasValue && !double.IsNaN(p.ReactionTime.Value))
                .Select(p => p.ReactionTime.Value);
            var loc = App.Current.Resources["Loc"] as StroopApp.Core.LocalizedStrings;
            GroupAverageLabel = string.Format(loc?["LiveAverage_Label"] ?? "", start, end);
            GroupAverageValue = group.Any()
                ? $"{group.Average():N0} ms"
                : loc?["LiveAverage_NoData"];
            OnPropertyChanged(nameof(GroupAverageLabel));
            OnPropertyChanged(nameof(GroupAverageValue));
        }
    }
}
