using StroopApp.Models;
using StroopApp.ViewModels;
using StroopApp.Services;
using System.Collections.ObjectModel;

namespace StroopApp.Views
{
    public partial class ProfileEditorWindow
    {
        public ProfileEditorWindow(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles, IProfileService profileService)
        {
            InitializeComponent();
            var viewModel = new ProfileEditorViewModel(profile, profiles, profileService);
            viewModel.CloseAction = new Action(this.Close);
            DataContext = viewModel;
        }
    }
}
