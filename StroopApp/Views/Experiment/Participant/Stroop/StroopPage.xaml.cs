// StroopPage.xaml.cs
using System.Windows.Controls;
using System.Windows.Input;

using StroopApp.Models;
using StroopApp.Services.Navigation;

namespace StroopApp.Views.Experiment.Participant
{
	public partial class StroopPage : Page
	{
		private readonly StroopViewModel _viewModel;

		public StroopPage(INavigationService participantWindowNavigationService, ExperimentSettings settings)
		{
			InitializeComponent();
			_viewModel = new StroopViewModel(settings, participantWindowNavigationService);
			DataContext = _viewModel;
			this.KeyDown += StroopPage_KeyDown;
			Loaded += (s, e) => Keyboard.Focus(this);
		}

		private void StroopPage_KeyDown(object sender, KeyEventArgs e)
		{
			_viewModel.ProcessInput(e.Key);
		}
	}
}
