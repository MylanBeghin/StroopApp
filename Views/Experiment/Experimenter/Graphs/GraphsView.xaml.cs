using StroopApp.Models;
using System.Windows.Controls;

namespace StroopApp.Views.Experiment.Experimenter.Graphs
{
    public partial class GraphsView : UserControl
    {
        public GraphsView(ExperimentSettings settings)
        {
            InitializeComponent();
            var ColumnGraphView = new ColumnGraphView(settings);
            var GlobalGraphView = new GlobalGraphView(settings);
            var LiveReactionTimeView = new LiveReactionTimeView(settings);
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
