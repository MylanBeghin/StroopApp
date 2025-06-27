using System.Windows;
using System.Windows.Controls;

using StroopApp.Models;
using StroopApp.Services.Exportation;
using StroopApp.ViewModels.Configuration;
using StroopApp.ViewModels.Experiment.Experimenter.End;
using StroopApp.Views.Configuration;

namespace StroopApp.Views.Experiment.Experimenter.End
{
	/// <summary>
	/// Logique d'interaction pour ExportEndExperimentWindow.xaml
	/// </summary>
	public partial class ExportEndExperimentWindow : Window
	{
		public ExportEndExperimentWindow(ExperimentSettings Settings, IExportationService exportationService)
		{
			InitializeComponent();
			DataContext = new ExportEndExperimentWindowViewModel(Settings, exportationService, this);
		}

	}
}
