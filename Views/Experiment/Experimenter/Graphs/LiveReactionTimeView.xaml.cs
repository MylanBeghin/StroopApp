using StroopApp.Models;
using StroopApp.ViewModels.Experiment.Experimenter;
using System.Windows.Controls;


namespace StroopApp.Views.Experiment.Experimenter.Graphs
{
    public partial class LiveReactionTimeView : UserControl
    {
        private readonly ExperimentSettings _settings;

        public LiveReactionTimeView(ExperimentSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            DataContext = new LiveReactionTimeViewModel(settings);
        }
    }
}
