using System.IO;
using System.Windows.Controls;

using StroopApp.Models;
using StroopApp.Services.Exportation;
using StroopApp.Services.Language;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.ViewModels.Experiment.Experimenter;
using StroopApp.ViewModels.Experiment.Experimenter.End;
using StroopApp.Views.Experiment.Experimenter.Graphs;

namespace StroopApp.Views.Experiment.Experimenter
{
	public partial class EndExperimentPage : Page
	{

		public EndExperimentPage(ExperimentSettings settings, INavigationService experimenterNavigationService, IWindowManager windowManager, ILanguageService languageService)
		{
			InitializeComponent();
			var configDir = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"StroopApp");
			var exportationService = new ExportationService(settings, languageService, configDir);
			DataContext = new EndExperimentPageViewModel(settings, exportationService, experimenterNavigationService, windowManager, languageService);
			var globalGraph = new GlobalGraphView(settings);
			MainGrid.Children.Add(globalGraph);
			Grid.SetRow(globalGraph, 4);
			Grid.SetColumnSpan(globalGraph, 3);
			var liveReactionTimeView = new LiveReactionTimeView(settings);
			MainGrid.Children.Add(liveReactionTimeView);
			Grid.SetRow(liveReactionTimeView, 2);
			Grid.SetColumn(liveReactionTimeView, 2);
		}
	}
}
