using StroopApp.Models;
using System.Windows;
using System.Windows.Controls;
using StroopApp.Services.Exportation;

namespace StroopApp.Views.Experiment.Experimenter
{
    public partial class CustomDialogPage : Page
    {
        readonly ExperimentSettings _settings;
        public IExportationService exportationService { get; set; }
        public CustomDialogPage(ExperimentSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            exportationService = new ExportationService(settings);
        }

        private void OnRestartClicked(object sender, RoutedEventArgs e)
        {
            _settings.ExperimentContext.NextAction = ExperimentAction.RestartBlock;
        }

        private async void OnNewExperimentClicked(object sender, RoutedEventArgs e)
        {
            await exportationService.ExportDataAsync();
            _settings.ExperimentContext.NextAction = ExperimentAction.NewExperiment;
        }

        private async void OnQuitClicked(object sender, RoutedEventArgs e)
        {
            await exportationService.ExportDataAsync();
            _settings.ExperimentContext.NextAction = ExperimentAction.Quit;
        }
    }
}
