using System.Windows;
using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Views.Experiment.Participant;
using StroopApp.ViewModels.Experiment.Participant;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;

namespace StroopApp.Views
{
    public partial class ParticipantWindow : Window
    {
        private readonly ExperimentSettings _settings;
        private readonly INavigationService _participantWindowNavigationService;
        public ParticipantWindow(ExperimentSettings Settings)
        {
            InitializeComponent();
            _participantWindowNavigationService = new NavigationService(ParticipantFrame);
            DataContext = new ParticipantWindowViewModel(Settings, _participantWindowNavigationService);
            _settings = Settings;
        }

        

        public void Reset()
        {
            DataContext = new ParticipantWindowViewModel(_settings, _participantWindowNavigationService);
        }
    }
}
