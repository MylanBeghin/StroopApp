using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

using StroopApp.ViewModels.Configuration.Profile;

namespace StroopApp.Views
{
	public partial class ProfileEditorWindow : Window
	{
		public ProfileEditorWindow(ProfileEditorViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;

			var switchVM = viewModel.SwitchSettingsViewModel;

			switchVM.DominantPercent = viewModel.Profile.DominantPercent;
			switchVM.SwitchPercent = viewModel.Profile.SwitchPercent;

			switchVM.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(switchVM.DominantPercent))
					viewModel.Profile.DominantPercent = switchVM.DominantPercent;
				if (e.PropertyName == nameof(switchVM.SwitchPercent))
					viewModel.Profile.SwitchPercent = switchVM.SwitchPercent;
			};

			viewModel.Profile.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(viewModel.Profile.DominantPercent))
					switchVM.DominantPercent = viewModel.Profile.DominantPercent;
				if (e.PropertyName == nameof(viewModel.Profile.SwitchPercent))
					switchVM.SwitchPercent = viewModel.Profile.SwitchPercent;
			};

			viewModel.CloseAction = () =>
			{
				DialogResult = viewModel.DialogResult;
				Close();
			};
		}
	}
}


