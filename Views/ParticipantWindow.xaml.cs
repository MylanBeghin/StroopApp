using System.Windows;
using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Views.Experiment.Participant;
using StroopApp.ViewModels.Experiment.Participant;

namespace StroopApp.Views
{
    public partial class ParticipantWindow : Window
    {
        private readonly INavigationService _navigationService;
        public ParticipantWindow(ExperimentSettings Settings)
        {
            InitializeComponent();
            _navigationService = new NavigationService(ExperimentFrame);
            DataContext = new ParticipantWindowViewModel(Settings, _navigationService);
            ExperimentFrame.Navigate(new InstructionsPage(Settings, _navigationService));
        }
    }
}
