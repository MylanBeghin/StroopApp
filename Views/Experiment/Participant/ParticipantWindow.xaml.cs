using System.Windows;
using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Views.Experiment.Participant;
namespace StroopApp.Views
{
    public partial class ParticipantWindow : Window
    {
        private readonly INavigationService _navigationService;
        public ParticipantWindow(ExperimentSettings Settings)
        {
            InitializeComponent();
            DataContext = Settings;
            _navigationService = new NavigationService(ExperimentFrame);
            ExperimentFrame.Navigate(new InstructionsPage(Settings, _navigationService));
        }
    }
}
