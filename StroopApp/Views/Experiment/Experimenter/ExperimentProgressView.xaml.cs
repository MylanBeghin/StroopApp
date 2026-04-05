using StroopApp.ViewModels.Experiment.Experimenter;
using StroopApp.ViewModels.State;
using System.Windows.Controls;
namespace StroopApp.Views.Experiment.Experimenter
{
    public partial class ExperimentProgressView : UserControl
    {
        public ExperimentProgressView(ExperimentSettingsViewModel Settings)
        {
            InitializeComponent();
            DataContext = new ExperimentProgressViewModel(Settings);
            Unloaded += (s, e) => (DataContext as IDisposable)?.Dispose();
        }
    }
}