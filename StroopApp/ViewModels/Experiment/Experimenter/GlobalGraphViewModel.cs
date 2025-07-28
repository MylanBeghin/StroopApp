using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

using StroopApp.Models;
using StroopApp.Resources;

namespace StroopApp.ViewModels.Experiment.Experimenter.Graphs
{
	public partial class GlobalGraphViewModel : ObservableObject
	{
		public ObservableCollection<ISeries> Series
		{
			get;
		}
		public ObservableCollection<RectangularSection> Sections
		{
			get;
		}
		public Axis[] XAxes
		{
			get;
		}
		public Axis[] YAxes
		{
			get;
		}
		public GlobalGraphViewModel(ExperimentSettings settings)
		{
			Series = settings.ExperimentContext.BlockSeries;
			Sections = settings.ExperimentContext.Sections;
			var totalTrials = settings.ExperimentContext.Blocks.Sum(b => b.TrialRecords.Count()) + settings.CurrentProfile.WordCount;
			if (settings.ExperimentContext.IsBlockFinished)
				totalTrials -= settings.CurrentProfile.WordCount;
			XAxes = new[]
			{
				new Axis
				{
					MinLimit = 0.5,
					MaxLimit = totalTrials + 0.5,
					MinStep = 1,
					Name = Strings.Header_Trials,
					NamePadding = new LiveChartsCore.Drawing.Padding(0),
					NameTextSize = 14
				}
		};

			double maxRt = settings.ExperimentContext.Blocks
	.SelectMany(b => b.TrialRecords)
	.Where(t => t.ReactionTime.HasValue)
	.Select(t => t.ReactionTime.Value)
	.DefaultIfEmpty(0)
	.Max();
			YAxes = new[]
				{
				new Axis {
					MinLimit = 0,
					MaxLimit = Math.Max(maxRt, settings.CurrentProfile.MaxReactionTime),
					Name = Strings.Header_ResponseTime,
					NamePadding = new LiveChartsCore.Drawing.Padding(0),
					NameTextSize = 14
	}
};
		}

	}
}
