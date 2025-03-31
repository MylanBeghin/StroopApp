using StroopApp.Models;
using System.Windows.Controls;
using StroopApp.Views.Experiment;
using StroopApp.Services.Navigation;
using StroopApp.ViewModels.Experiment;
namespace StroopApp.Views
{
    public partial class ExperimentDashBoardPage : Page
    {
        public ExperimentDashBoardPage(INavigationService navigationService, ExperimentSettings sharedSettings)
        {
            InitializeComponent();
            
            var ExperimentProfileView = new ExperimentProfileView(sharedSettings.CurrentProfile);

            DataContext = new ExperimentDashBoardPageViewModel(navigationService, sharedSettings);
            MainGrid.Children.Add(ExperimentProfileView);
            Grid.SetRow(ExperimentProfileView, 1);
        }
    }
}
