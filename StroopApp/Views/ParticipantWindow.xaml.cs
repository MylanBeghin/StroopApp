using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Services.Navigation.PageFactory;
using StroopApp.ViewModels.Experiment.Participant;
using System.Windows;

namespace StroopApp.Views
{
    public partial class ParticipantWindow : Window
    {
        private readonly ExperimentSettings _settings;
        private readonly INavigationService _participantNavigationService;
        private readonly IPageFactory _pageFactory;

        public ParticipantWindow(ExperimentSettings settings, IPageFactory pageFactory)
        {
            InitializeComponent();
            _settings = settings;
            _pageFactory = pageFactory;
            var navigationService = new NavigationService(pageFactory);
            navigationService.SetFrame(ParticipantFrame);
            _participantNavigationService = navigationService;
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
