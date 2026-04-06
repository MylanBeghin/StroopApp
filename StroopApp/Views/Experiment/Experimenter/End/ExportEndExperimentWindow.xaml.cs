using StroopApp.Services.Exportation;
using StroopApp.Services.Navigation;
using StroopApp.ViewModels.Experiment.Experimenter.End;
using StroopApp.ViewModels.State;
using System.Windows;

namespace StroopApp.Views.Experiment.Experimenter.End
{
	public partial class ExportEndExperimentWindow : Window
	{
		public ExportEndExperimentWindow(ExperimentSettingsViewModel Settings, IExportationService exportationService, INavigationService experimenterNavigationService)
		{
			InitializeComponent();
			DataContext = new ExportEndExperimentWindowViewModel(Settings, exportationService, closeWindow: () => Close(), setDialogResult: result => DialogResult = result, experimenterNavigationService);
		}
	}
}
