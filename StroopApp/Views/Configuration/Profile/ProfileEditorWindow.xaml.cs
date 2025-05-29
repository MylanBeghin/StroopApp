using System.Windows;

using StroopApp.ViewModels.Configuration.Profile;

namespace StroopApp.Views
{
	public partial class ProfileEditorWindow : Window
	{
		public SwitchSettingsViewModel SwitchSettingsViewModel
		{
			get;
		}
		public ProfileEditorWindow(ProfileEditorViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;

			SwitchSettingsViewModel = new SwitchSettingsViewModel();

			SwitchSettingsViewModel.DominantForm = viewModel.Profile.SelectedDominantForm ?? "Aucune";
			SwitchSettingsViewModel.DominantPercent = viewModel.Profile.DominantPercent;

			SwitchSettingsViewModel.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(SwitchSettingsViewModel.DominantForm))
					viewModel.Profile.SelectedDominantForm = SwitchSettingsViewModel.DominantForm;
				if (e.PropertyName == nameof(SwitchSettingsViewModel.DominantPercent))
					viewModel.Profile.DominantPercent = SwitchSettingsViewModel.DominantPercent;
			};

			viewModel.CloseAction = () =>
			{
				DialogResult = viewModel.DialogResult;
				Close();
			};
		}
	}
}
