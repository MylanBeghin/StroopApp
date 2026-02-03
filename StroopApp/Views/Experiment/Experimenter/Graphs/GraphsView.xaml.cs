using System.Windows.Controls;

using StroopApp.Models;

namespace StroopApp.Views.Experiment.Experimenter.Graphs
{
	public partial class GraphsView : UserControl
	{
		public GraphsView(ExperimentSettings settings)
		{
			InitializeComponent();
			var columnGraphView = new ColumnGraphView(settings);
			var globalGraphView = new GlobalGraphView(settings);
			var liveReactionTimeView = new LiveReactionTimeView(settings);
			MainGrid.Children.Add(globalGraphView);
			Grid.SetRow(globalGraphView, 2);
			Grid.SetColumnSpan(globalGraphView, 3);
			Grid.SetColumn(globalGraphView, 0);
			MainGrid.Children.Add(columnGraphView);
			Grid.SetRow(columnGraphView, 4);
			Grid.SetColumn(columnGraphView, 0);
			MainGrid.Children.Add(liveReactionTimeView);
			Grid.SetRow(liveReactionTimeView, 4);
			Grid.SetColumn(liveReactionTimeView, 2);
		}
	}
}
