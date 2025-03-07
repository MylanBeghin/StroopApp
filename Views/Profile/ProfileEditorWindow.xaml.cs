using StroopApp.Models;
using StroopApp.ViewModels;
using StroopApp.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace StroopApp.Views
{
    public partial class ProfileEditorWindow : Window
    {
        public ProfileEditorWindow(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles, IProfileService profileService)
        {
            InitializeComponent();
            var viewModel = new ProfileEditorViewModel(profile, profiles, profileService);
            DataContext = viewModel;
            viewModel.CloseAction = () =>
            {
                DialogResult = viewModel.DialogResult;
                Close();
            };
        }
    }
}

