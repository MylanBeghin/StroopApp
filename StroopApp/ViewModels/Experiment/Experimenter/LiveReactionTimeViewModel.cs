using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

using StroopApp.Core;
using StroopApp.Models;

namespace StroopApp.ViewModels.Experiment.Experimenter
{
	internal class LiveReactionTimeViewModel : ViewModelBase
	{
		private readonly ExperimentSettings _settings;

		public ObservableCollection<ReactionTimePoint> ReactionPoints { get; }
		public ObservableCollection<ReactionGroup> GroupAverages { get; } = new();

		private int _lastCompletedGroupIndex = 0;

		public int GroupSize => _settings.CurrentProfile.GroupSize;

		public LiveReactionTimeViewModel(ExperimentSettings settings)
		{
			_settings = settings;
			ReactionPoints = _settings.ExperimentContext.ReactionPoints;

			_settings.ExperimentContext.ReactionPoints.CollectionChanged += ReactionPoints_CollectionChanged;
			if (_settings.CurrentProfile is INotifyPropertyChanged npc)
			{
				npc.PropertyChanged += (s, e) =>
				{
					if (e.PropertyName == nameof(ExperimentProfile.GroupSize))
					{
						OnPropertyChanged(nameof(GroupSize));
						RecomputeAllGroupsAfterGroupSizeChange();
					}
				};
			}
		}

		private void ReactionPoints_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				TryAddCompletedGroup();
			}
			else if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				GroupAverages.Clear();
				_lastCompletedGroupIndex = 0;
			}
		}

		private void TryAddCompletedGroup()
		{
			if (_settings?.CurrentProfile == null)
				return;

			int groupSize = _settings.CurrentProfile.GroupSize;
			if (groupSize <= 0)
				groupSize = 1;

			int total = ReactionPoints.Count;
			if (total == 0)
				return;
			if (total % groupSize != 0)
				return;

			int currentGroupIndex = total / groupSize;
			if (currentGroupIndex <= _lastCompletedGroupIndex)
				return;

			AddGroup(currentGroupIndex, groupSize);
		}

		private void AddGroup(int groupIndex, int groupSize)
		{
			int startTrial = (groupIndex - 1) * groupSize + 1;
			int endTrial = groupIndex * groupSize;

			var slice = ReactionPoints
				.Skip(startTrial - 1)
				.Take(groupSize)
				.ToList();

			var validTimes = slice
				.Where(p => p.ReactionTime.HasValue && !double.IsNaN(p.ReactionTime.Value))
				.Select(p => p.ReactionTime!.Value)
				.ToList();

			double? average = validTimes.Count > 0 ? validTimes.Average() : (double?)null;
			int correct = slice.Count(p => p.IsValidResponse == true);

			GroupAverages.Add(new ReactionGroup(startTrial, endTrial, average, correct, groupSize));
			_lastCompletedGroupIndex = groupIndex;
		}
		private void RecomputeAllGroupsAfterGroupSizeChange()
		{
			GroupAverages.Clear();
			_lastCompletedGroupIndex = 0;

			int groupSize = GroupSize <= 0 ? 1 : GroupSize;
			int total = ReactionPoints.Count;
			if (total == 0)
				return;

			int fullGroupCount = total / groupSize;
			for (int gi = 1; gi <= fullGroupCount; gi++)
				AddGroup(gi, groupSize);
		}
	}
}
