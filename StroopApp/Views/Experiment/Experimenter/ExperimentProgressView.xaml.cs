using StroopApp.Models;
using StroopApp.ViewModels.Experiment.Experimenter;
using System.Windows.Controls;
namespace StroopApp.Views.Experiment.Experimenter
{
    public partial class ExperimentProgressView : UserControl
    {
        public ExperimentProgressView(ExperimentSettings Settings)
        {
            InitializeComponent();
            DataContext = new ExperimentProgressViewModel(Settings);
        }
    }
}