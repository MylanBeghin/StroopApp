using System.Windows.Controls;
using StroopApp.Models;
using StroopApp.ViewModels.Experiment;
namespace StroopApp.Views.Experiment
{
    public partial class ExperimentProgressView : UserControl
    {
        public ExperimentProgressView(ExperimentProfile Settings)
        {
            InitializeComponent();
            DataContext = new ExperimentProgressViewModel(Settings);
        }
    }
}
