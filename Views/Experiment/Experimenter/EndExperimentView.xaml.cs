using StroopApp.Models;
using System.Windows.Controls;
using StroopApp.ViewModels.Experiment.Experimenter;

namespace StroopApp.Views.Experiment.Experimenter
{
    public partial class EndExperimentView : Page
    {
        public EndExperimentView(ExperimentSettings settings)
        {
            InitializeComponent();
            DataContext = new EndExperimentViewModel(settings);
        }
    }
}
