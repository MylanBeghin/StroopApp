using StroopApp.ViewModels.Configuration.Profile;
using System.Windows;

namespace StroopApp.Views
{
    public partial class ProfileEditorWindow : Window
    {
        public ProfileEditorWindow(ProfileEditorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.CloseAction = () =>
            {
                DialogResult = viewModel.DialogResult;
                Close();
            };
        }
    }
}
