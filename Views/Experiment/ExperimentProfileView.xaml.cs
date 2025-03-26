using System.Windows.Controls;
using StroopApp.Models;
namespace StroopApp.Views.Experiment
{
    public partial class ExperimentProfileView : UserControl
    {
        public ExperimentProfileView(ExperimentProfile Settings)
        {
            InitializeComponent();
            DataContext = Settings;
        }
    }
}
