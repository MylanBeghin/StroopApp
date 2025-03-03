using StroopApp.Models;
using StroopApp.ViewModels;
using System.Windows;
using StroopApp.Services.Profile;
using StroopApp.Services;
using System.Collections.ObjectModel;

namespace StroopApp.Views
{
    /// <summary>
    /// Logique d'interaction pour ProfileEditorWindow.xaml
    /// </summary>
    public partial class ProfileEditorWindow : Window
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
