using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.ViewModels.Experiment.Participant;
using StroopApp.ViewModels.State;
using System.Windows;

namespace StroopApp.Views
{
    public partial class ParticipantWindow : Window
    {
        private readonly ExperimentSettingsViewModel _settings;
        private readonly IParticipantNavigationService _participantNavigationService;

        public ParticipantWindow(ExperimentSettingsViewModel settings, IParticipantNavigationService participantNavigationService)
        {
            InitializeComponent();
            _settings = settings;
            _participantNavigationService = participantNavigationService;
            _participantNavigationService.SetFrame(ParticipantFrame);
            DataContext = new ParticipantWindowViewModel(settings, _participantNavigationService);
        }

        public void Reset()
        {
            if (DataContext is ParticipantWindowViewModel oldViewModel)
            {
                oldViewModel.Dispose();
            }
            DataContext = new ParticipantWindowViewModel(_settings, _participantNavigationService);
        }
    }

}
