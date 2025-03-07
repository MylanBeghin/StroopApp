using System.Windows.Controls;
using StroopApp.Services.Profile;
using StroopApp.ViewModels;

namespace StroopApp.Views.Profile
{
    public partial class ProfileManagementView : UserControl
    {
        public ProfileManagementView()
        {
            ProfileService profileService = new ProfileService();
            InitializeComponent();
            DataContext = new ProfileManagementViewModel(profileService);
        }
    }
}
