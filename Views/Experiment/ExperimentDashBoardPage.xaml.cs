using StroopApp.Models;
using System.Windows.Controls;
using StroopApp.Views.Experiment;
namespace StroopApp.Views
{
    public partial class ExperimentDashBoardPage : Page
    {
        public ExperimentDashBoardPage(ExperimentSettings Settings)
        {
            InitializeComponent();
            DataContext = Settings;
            var ExperimentProfileView = new ExperimentProfileView(Settings.CurrentProfile);
            MainGrid.Children.Add(ExperimentProfileView);
            Grid.SetRow(ExperimentProfileView, 1);
        }
    }
}
