using StroopApp.Models;
using StroopApp.Services.Exportation;
using StroopApp.ViewModels.Experiment.Experimenter;
using System.Windows.Controls;

namespace StroopApp.Views.Experiment.Experimenter
{
    public partial class EndExperimentPage : Page
    {
        public IExportationService exportationService { get; set; }
        public EndExperimentPage(ExperimentSettings settings)
        {
            InitializeComponent();
            exportationService = new ExportationService(settings);
            DataContext = new EndExperimentViewModel(settings, exportationService);

        }
    }
}
