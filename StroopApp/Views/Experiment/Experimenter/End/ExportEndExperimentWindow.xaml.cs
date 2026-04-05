using StroopApp.Models;
using StroopApp.Services.Exportation;
using StroopApp.Services.Navigation;
using StroopApp.ViewModels.Configuration;
using StroopApp.ViewModels.Experiment.Experimenter.End;
using StroopApp.ViewModels.State;
using StroopApp.Views.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace StroopApp.Views.Experiment.Experimenter.End
{
	public partial class ExportEndExperimentWindow : Window
	{
		public ExportEndExperimentWindow(ExperimentSettingsViewModel Settings, IExportationService exportationService, INavigationService experimenterNavigationService)
		{
			InitializeComponent();
			DataContext = new ExportEndExperimentWindowViewModel(Settings, exportationService, closeWindow: () => Close(), setDialogResult: result => DialogResult = result, experimenterNavigationService);
			var folderSelectorVM = new ExportFolderSelectorViewModel(Settings, exportationService);
			var folderSelectorView = new ExportFolderSelectorView(folderSelectorVM);

			EndGrid.Children.Add(folderSelectorView);
			Grid.SetRow(folderSelectorView, 0);
		}

	}
}
