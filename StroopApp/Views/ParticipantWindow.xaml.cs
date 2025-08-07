using System.Windows;

using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.ViewModels.Experiment.Participant;

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
			if (DataContext is ParticipantWindowViewModel oldViewModel)
			{
				oldViewModel.Dispose();
			}

			DataContext = new ParticipantWindowViewModel(_settings, _participantWindowNavigationService);
		}
	}
}
