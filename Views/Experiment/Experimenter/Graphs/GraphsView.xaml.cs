using System.Windows.Controls;
using StroopApp.Models;
using StroopApp.ViewModels.Experiment.Experimenter;

namespace StroopApp.Views.Experiment.Experimenter.Graphs
{
    public partial class GraphsView : UserControl
    {
        public GraphsView(ExperimentSettings settings)
        {
            InitializeComponent();
            var GraphVM = new ExperimentGraphViewModel(settings);
            var ColumnGraphView = new ColumnGraphView(GraphVM);
            var GlobalGraphView = new GlobalGraphView(GraphVM);
            var LiveReactionTimeView = new LiveReactionTimeView(GraphVM);
            MainGrid.Children.Add(GlobalGraphView);
            Grid.SetRow(GlobalGraphView, 2);
            Grid.SetColumn(GlobalGraphView, 0);
            MainGrid.Children.Add(ColumnGraphView);
            Grid.SetRow(ColumnGraphView, 4);
            Grid.SetColumn(ColumnGraphView, 0);
            MainGrid.Children.Add(LiveReactionTimeView);
            Grid.SetRow(LiveReactionTimeView, 2);
            Grid.SetRowSpan(LiveReactionTimeView, 3);
            Grid.SetColumn(LiveReactionTimeView, 2);
        }
    }
}
