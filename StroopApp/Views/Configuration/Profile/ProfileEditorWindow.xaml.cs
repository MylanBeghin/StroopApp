using System.Windows;

using StroopApp.ViewModels.Configuration.Profile;

namespace StroopApp.Views
{
	public partial class ProfileEditorWindow : Window
	{
		public ProfileEditorWindow(ProfileEditorViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;

			// Utilise le SwitchSettingsViewModel du viewModel
			var switchVM = viewModel.SwitchSettingsViewModel;

			// Synchronise les valeurs du profil vers le VM d'UI
			switchVM.DominantPercent = viewModel.Profile.DominantPercent;
			switchVM.SwitchPercent = viewModel.Profile.SwitchPercent;

			// Quand l'utilisateur modifie dans l'UI, on met à jour le profil
			switchVM.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(switchVM.DominantPercent))
					viewModel.Profile.DominantPercent = switchVM.DominantPercent;
				if (e.PropertyName == nameof(switchVM.SwitchPercent))
					viewModel.Profile.SwitchPercent = switchVM.SwitchPercent;
			};

			// Quand le profil change (par code), on met à jour le VM d'UI
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
