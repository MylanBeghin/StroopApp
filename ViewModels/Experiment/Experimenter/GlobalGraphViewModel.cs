using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using StroopApp.Models;
using System.Collections.ObjectModel;

namespace StroopApp.ViewModels.Experiment.Experimenter.Graphs
{
    public partial class GlobalGraphViewModel : ObservableObject
    {
        public ObservableCollection<RectangularSection> Sections { get; }
            = new ObservableCollection<RectangularSection>();

        public IEnumerable<ISeries> Series { get; }
        public Axis[] XAxes { get; }
        public Axis[] YAxes { get; }

        public GlobalGraphViewModel(ExperimentSettings settings)
        {
            // sections colorées
            var sectionPalette = new[]
            {
                new SKColor(179, 229, 252, 120),
                new SKColor(200, 230, 201, 120),
                new SKColor(255, 249, 196, 120),
                new SKColor(255, 204, 188, 120),
                new SKColor(209, 196, 233, 120)
            };

            int secStart = 1;
            int secIdx = 0;
            foreach (var block in settings.ExperimentContext.Blocks)
            {
                int secEnd = secStart + block.TotalTrials - 1;
                Sections.Add(new RectangularSection
                {
                    Xi = secStart,
                    Xj = secEnd,
                    Fill = new SolidColorPaint(sectionPalette[secIdx % sectionPalette.Length]) { ZIndex = 0 },
                    Label = $"Bloc {block.BlockNumber} - {block._profileName}",
                    LabelPaint = new SolidColorPaint(SKColors.Black) { ZIndex = 1 },
                    LabelSize = 14
                });
                secStart = secEnd + 1;
                secIdx++;
            }

            // séries par bloc
            var seriesPalette = new[]
            {
                new SKColor(66, 133, 244),
                new SKColor(219,  68,  55),
                new SKColor(244, 180,   0),
                new SKColor( 15, 157,  88),
                new SKColor(171,  71, 188)
            };

            var seriesList = new List<ISeries>();
            int valStart = 0;
            int palIdx = 0;
            foreach (var block in settings.ExperimentContext.Blocks)
            {
                int valEnd = secStart + block.TotalTrials - 1;
                var vals = settings.ExperimentContext.ReactionTimes
                    .Skip(valStart)
                    .Take(block.TotalTrials)
                    .Select(rt => rt ?? double.NaN)
                    .ToArray();

                seriesList.Add(new LineSeries<double>
                {
                    Values = vals,
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint(sectionPalette[secIdx % sectionPalette.Length]) { ZIndex = 0 },
                    Mapping = (value, index) => new Coordinate(valStart + index + 1, value)
                });
                valStart += block.TotalTrials;
                palIdx++;
            }
            Series = seriesList;

            // axes
            int total = settings.ExperimentContext.TrialRecords.Count() + settings.CurrentProfile.WordCount;
            XAxes = new[]
            {
                new Axis { MinLimit = 0.5, MaxLimit = total + 0.5, MinStep = 1 }
            };

            double maxRt = settings.ExperimentContext.TrialRecords
                .Where(t => t.ReactionTime.HasValue)
                .Select(t => t.ReactionTime.Value)
                .DefaultIfEmpty(0)
                .Max();
            YAxes = new[]
            {
                new Axis { MinLimit = 0, MaxLimit = Math.Max(maxRt, settings.CurrentProfile.MaxReactionTime) }
            };
        }
    }
}
