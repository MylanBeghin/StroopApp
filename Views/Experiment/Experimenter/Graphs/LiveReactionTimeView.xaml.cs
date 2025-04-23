using StroopApp.ViewModels.Experiment.Experimenter;
using System.Windows.Controls;


namespace StroopApp.Views.Experiment.Experimenter.Graphs
{
    public partial class LiveReactionTimeView : UserControl
    {
        public LiveReactionTimeView(ExperimentGraphViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
