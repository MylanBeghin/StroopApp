using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Documents;
using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.ViewModels.Experiment;

namespace StroopApp.Views.Experiment.Participant
{
    public partial class InstructionsPage : Page
    {
        private readonly InstructionsPageViewModel _viewModel;
        public InstructionsPage(ExperimentSettings settings, INavigationService navigationService)
        {
            InitializeComponent();
            _viewModel = new InstructionsPageViewModel(settings, navigationService);
            DataContext = _viewModel;
            _viewModel.InstructionChanged += (s, e) => InstructionContentControl.Content = _viewModel.CurrentInstruction;
            Loaded += (s, e) =>
            {
                InstructionContentControl.Content = _viewModel.CurrentInstruction;
                Keyboard.Focus(this);
            };
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                _viewModel.NextCommand.Execute(null);
        }
    }
}
