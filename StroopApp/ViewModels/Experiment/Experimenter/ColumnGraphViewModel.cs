using System.Collections.ObjectModel;

using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;

using StroopApp.Models;

namespace StroopApp.ViewModels.Experiment.Experimenter
{

    internal class ColumnGraphViewModel
    {
        public ObservableCollection<ISeries> ColumnSerie
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
        public Func<ChartPoint, string> TooltipFormatter => chartPoint =>
        {
            if (chartPoint.Context.DataSource is not ReactionTimePoint model)
                return string.Empty;

            var trial = $"Essai : {model.TrialNumber}";

            if (model.ReactionTime is null)
                return $"{trial}\nPas de réponse";

            var rt = $"Temps de réponse : {model.ReactionTime.Value:N0} ms";
            var validity = model.IsValidResponse switch
            {
                true => "Réponse correcte",
                false => "Réponse incorrecte",
                _ => "Validité inconnue"
            };

            return $"{trial}\n{rt}\n{validity}";
        };



        public ColumnGraphViewModel(ExperimentSettings settings)
        {
            ColumnSerie = settings.ExperimentContext.ColumnSerie;
            XAxes = new[]
            {
                new Axis
                {
                    MinLimit = -0.5,
                    MaxLimit = 10,
                    MinStep = 1,
                    Labeler = value => ((int)(value + 1)).ToString(),
                    Name = "Essais",
                    NamePadding = new LiveChartsCore.Drawing.Padding(0),
                    NameTextSize = 14
                }
            };
            YAxes = new[]
            {
                new Axis
                {
                    MinLimit = 0,
                    MaxLimit = settings.CurrentProfile.MaxReactionTime * 1.1,
                    Name = "Temps de réponse (ms)",
                    NamePadding = new LiveChartsCore.Drawing.Padding(0),
                    NameTextSize = 14
                }
            };
            settings.ExperimentContext.ReactionPoints.CollectionChanged += (s, e) =>
            {
                int count = settings.ExperimentContext.ReactionPoints.Count;
                const int window = 10;

                if (count > window)
                {
                    XAxes.First().MinLimit = count - window;
                    XAxes.First().MaxLimit = count;
                }
                ;
            };

        }
    }
}