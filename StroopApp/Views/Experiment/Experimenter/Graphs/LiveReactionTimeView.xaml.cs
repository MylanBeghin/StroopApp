using System.Windows.Controls;

namespace StroopApp.Views.Experiment.Experimenter.Graphs
{
	public partial class LiveReactionTimeView : UserControl
	{
		public LiveReactionTimeView()
		{
			InitializeComponent();
            Unloaded += (s, e) => (DataContext as IDisposable)?.Dispose();
        }
	}
}
