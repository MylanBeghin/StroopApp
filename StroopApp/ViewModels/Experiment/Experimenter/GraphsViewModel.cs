using StroopApp.ViewModels.State;

namespace StroopApp.ViewModels.Experiment.Experimenter
{
    /// <summary>
    /// Aggregates the three graph sub-ViewModels shown on the experiment dashboard.
    /// Owns the sub-ViewModels and disposes them when finished.
    /// </summary>
    public class GraphsViewModel : IDisposable
    {
        public ColumnGraphViewModel ColumnGraphViewModel { get; }
        public GlobalGraphViewModel GlobalGraphViewModel { get; }
        public LiveReactionTimeViewModel LiveReactionTimeViewModel { get; }

        public GraphsViewModel(ExperimentSettingsViewModel settings)
        {
            ColumnGraphViewModel = new ColumnGraphViewModel(settings);
            GlobalGraphViewModel = new GlobalGraphViewModel(settings);
            LiveReactionTimeViewModel = new LiveReactionTimeViewModel(settings);
        }

        public void Dispose()
        {
            (ColumnGraphViewModel as IDisposable)?.Dispose();
            (GlobalGraphViewModel as IDisposable)?.Dispose();
            LiveReactionTimeViewModel.Dispose();
        }
    }
}
