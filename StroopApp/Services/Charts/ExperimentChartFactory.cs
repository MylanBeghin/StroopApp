using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using StroopApp.Models;

namespace StroopApp.Services.Charts
{
	/// <summary>
	/// Factory responsible for creating LiveCharts graphics objects.
	/// Extracted from SharedExperimentData to isolate LiveCharts logic.
	/// This is a mechanical extraction - behavior is preserved EXACTLY as it was.
	/// </summary>
	public class ExperimentChartFactory
	{
		/// <summary>
		/// Creates a live-bound column series for reaction time points.
		/// CRITICAL: Values references the live collection (not a copy).
		/// This is the exact behavior extracted from SharedExperimentData.NewColumnSerie().
		/// </summary>
		/// <param name="reactionPoints">Live collection to bind to (modifications propagate to chart)</param>
		/// <returns>ObservableCollection containing exactly 1 ColumnSeries</returns>
		public ObservableCollection<ISeries> CreateLiveColumnSerie(ObservableCollection<ReactionTimePoint> reactionPoints)
		{
			return new ObservableCollection<ISeries>
			{
				new ColumnSeries<ReactionTimePoint>
				{
					Values = reactionPoints,
					DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
					DataLabelsSize = 16,
					DataLabelsPaint = new SolidColorPaint(SKColors.Black),
					DataLabelsFormatter = point => point.Coordinate.SecondaryValue.Equals(double.NaN) ? "Aucune réponse" : point.Coordinate.PrimaryValue.ToString("N0"),
					XToolTipLabelFormatter = point =>
					{
						var trial = (int)(point.Coordinate.SecondaryValue + 1);
						return $"Essai n°{trial}";
					},
					YToolTipLabelFormatter = point =>
					{
						var value = point.Coordinate.PrimaryValue;
						return double.IsNaN(value)
							? "Pas de réponse"
							: $"Temps de réponse : {value:0.#####} ms";
					},
					Mapping = (point, index) => new Coordinate(point.TrialNumber - 1, point.ReactionTime != null ? point.ReactionTime.Value : double.NaN)
				}.OnPointCreated(p =>
				{
					if (p.Visual is null) return;
					var model = p.Model;
					if (model != null && model.IsValidResponse.HasValue)
					{
						// Orange (wrong answer)
						var orange = new SKColor(255, 166, 0);      // #FFA600
						// Purple (right answer)
						var purple = new SKColor(91, 46, 255);      // #5B2EFF
						if (model.IsValidResponse.Value)
						{
							p.Visual.Fill = new SolidColorPaint(purple);
							p.Visual.Stroke = new SolidColorPaint(purple);
						}
						else
						{
							p.Visual.Fill = new SolidColorPaint(orange);
							p.Visual.Stroke = new SolidColorPaint(orange);
						}
					}
				})
			};
		}

		/// <summary>
		/// Creates a snapshot column series for reaction time points.
		/// CRITICAL: Values references a COPY of the data (not live).
		/// This is the exact behavior used in EndExperimentPageViewModel.UpdateBlock().
		/// </summary>
		/// <param name="snapshot">Snapshot collection (isolated from original)</param>
		/// <returns>ObservableCollection containing exactly 1 ColumnSeries</returns>
		public ObservableCollection<ISeries> CreateSnapshotColumnSerie(ObservableCollection<ReactionTimePoint> snapshot)
		{
			// EXACT same logic as CreateLiveColumnSerie, but different usage intent (snapshot vs live)
			return new ObservableCollection<ISeries>
			{
				new ColumnSeries<ReactionTimePoint>
				{
					Values = snapshot,
					DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
					DataLabelsSize = 16,
					DataLabelsPaint = new SolidColorPaint(SKColors.Black),
					DataLabelsFormatter = point =>
						point.Coordinate.PrimaryValue.Equals(null)
							? "Aucune réponse"
							: point.Coordinate.PrimaryValue.ToString("N0"),
					Mapping = (point, index) => new Coordinate(
						point.TrialNumber - 1,
						point.ReactionTime != null ? point.ReactionTime.Value : double.NaN
					)
				}
				.OnPointCreated(p =>
				{
					if (p.Visual is null) return;
					var model = p.Model;
					if (model != null && model.IsValidResponse.HasValue)
					{
						var orange = new SKColor(255, 166, 0);
						var violet = new SKColor(91, 46, 255);
						if (model.IsValidResponse.Value)
						{
							p.Visual.Fill = new SolidColorPaint(violet);
							p.Visual.Stroke = new SolidColorPaint(violet);
						}
						else
						{
							p.Visual.Fill = new SolidColorPaint(orange);
							p.Visual.Stroke = new SolidColorPaint(orange);
						}
					}
				})
			};
		}

		/// <summary>
		/// Creates a LineSeries for a block's trial times.
		/// This is the exact behavior extracted from SharedExperimentData.AddNewSerie().
		/// CRITICAL: Mapping uses pt.Value (assumes non-null values).
		/// </summary>
		/// <param name="trialTimes">Live collection of trial times from CurrentBlock</param>
		/// <param name="start">Start index for X mapping (currentBlockStart value)</param>
		/// <returns>LineSeries configured exactly as in original code</returns>
		public LineSeries<double?> CreateBlockLineSeries(ObservableCollection<double?> trialTimes, int start)
		{
			return new LineSeries<double?>
			{
				Values = trialTimes,
				Stroke = new SolidColorPaint(SKColors.Black, 2),
				Fill = new SolidColorPaint(SKColors.Black.WithAlpha(60)),
				LineSmoothness = 0.4f,
				GeometrySize = 6,
				GeometryStroke = new SolidColorPaint(SKColors.Black, 2),
				GeometryFill = new SolidColorPaint(SKColors.White),
				Mapping = (pt, idx) => new Coordinate(start + idx, pt.Value)
			};
		}

		/// <summary>
		/// Creates a RectangularSection for a block range.
		/// This is the exact behavior extracted from SharedExperimentData.AddNewSerie().
		/// </summary>
		/// <param name="xi">Start trial index</param>
		/// <param name="xj">End trial index</param>
		/// <param name="blockNumber">Block number for label</param>
		/// <param name="fillColor">Fill color with alpha (from palette)</param>
		/// <returns>RectangularSection configured exactly as in original code</returns>
		public RectangularSection CreateBlockSection(int xi, int xj, int blockNumber, SKColor fillColor)
		{
			return new RectangularSection
			{
				Xi = xi,
				Xj = xj,
				Fill = new SolidColorPaint(fillColor),
				Label = $"Bloc n°{blockNumber}",
				LabelSize = 16,
				LabelPaint = new SolidColorPaint(SKColors.Black)
			};
		}
	}
}
