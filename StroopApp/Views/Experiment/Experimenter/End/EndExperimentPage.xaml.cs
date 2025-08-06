using System.IO;
using System.Windows.Controls;

using StroopApp.Models;
using StroopApp.Services.Exportation;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.ViewModels.Experiment.Experimenter;
using StroopApp.ViewModels.Experiment.Experimenter.End;
using StroopApp.Views.Experiment.Experimenter.Graphs;

namespace StroopApp.Views.Experiment.Experimenter
{
	public partial class EndExperimentPage : Page
	{

		public EndExperimentPage(ExperimentSettings settings, INavigationService experimenterNavigationService, IWindowManager windowManager)
		{
			InitializeComponent();
			var configDir = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"StroopApp");
			var ExportationService = new ExportationService(settings, configDir);
			DataContext = new EndExperimentViewModel(settings, ExportationService, experimenterNavigationService, windowManager);
			var GlobalGraph = new GlobalGraphView(settings);
			MainGrid.Children.Add(GlobalGraph);
			Grid.SetRow(GlobalGraph, 4);
		}
	}
}
