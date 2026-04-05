using StroopApp.Models;
using StroopApp.ViewModels.Experiment.Experimenter;
using StroopApp.ViewModels.State;
using System.Windows.Controls;

namespace StroopApp.Views.Experiment.Experimenter.Graphs
{
	public partial class LiveReactionTimeView : UserControl
	{
		private readonly ExperimentSettingsViewModel _settings;

		public LiveReactionTimeView(ExperimentSettingsViewModel settings)
		{
			InitializeComponent();
			_settings = settings;
			DataContext = new LiveReactionTimeViewModel(settings);
            Unloaded += (s, e) => (DataContext as IDisposable)?.Dispose();
        }
	}
}
