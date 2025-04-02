using System.Windows.Controls;
using System.Windows.Input;
using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.ViewModels;

namespace StroopApp.Views.Experiment.Participant
{
    public partial class StroopPage : Page
    {
        private readonly StroopViewModel _viewModel;

        public StroopPage(INavigationService navigationService, ExperimentSettings settings)
        {
            InitializeComponent();
            _viewModel = new StroopViewModel(settings);
            DataContext = _viewModel;
            this.KeyDown += StroopPage_KeyDown;
        }

        private void StroopPage_KeyDown(object sender, KeyEventArgs e)
        {
            _viewModel.ProcessInput(e.Key);
        }
    }
}
