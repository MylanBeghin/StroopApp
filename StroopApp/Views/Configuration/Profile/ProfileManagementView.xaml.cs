using System.Windows.Controls;
using StroopApp.Services.Profile;
using StroopApp.ViewModels.Configuration.Profile;

namespace StroopApp.Views.Profile
{
    public partial class ProfileManagementView : UserControl
    {
        public ProfileManagementView(ProfileManagementViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
