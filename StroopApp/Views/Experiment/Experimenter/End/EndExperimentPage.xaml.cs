using System.Windows.Controls;

using StroopApp.Models;
using StroopApp.Services.Exportation;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.ViewModels.Experiment.Experimenter.End;
using StroopApp.Views.Experiment.Experimenter.Graphs;

namespace StroopApp.Views.Experiment.Experimenter
{
	public partial class EndExperimentPage : Page, INavigationAware
	{
		private readonly ExperimentSettings _settings;
		private readonly IExportationService _exportationService;
		private readonly IWindowManager _windowManager;

		public INavigationService NavigationService
		{
			set => Initialize(value);
		}

		public EndExperimentPage(ExperimentSettings settings, IExportationService exportationService, IWindowManager windowManager)
		{
			InitializeComponent();
			_settings = settings;
			_exportationService = exportationService;
			_windowManager = windowManager;
		}

		private void Initialize(INavigationService navigationService)
		{
			DataContext = new EndExperimentPageViewModel(_settings, _exportationService, navigationService, _windowManager);
			var globalGraph = new GlobalGraphView(_settings);
			MainGrid.Children.Add(globalGraph);
			Grid.SetRow(globalGraph, 4);
			Grid.SetColumnSpan(globalGraph, 3);
			var liveReactionTimeView = new LiveReactionTimeView(_settings);
			MainGrid.Children.Add(liveReactionTimeView);
			Grid.SetRow(liveReactionTimeView, 2);
			Grid.SetColumn(liveReactionTimeView, 2);
		}
	}
}
