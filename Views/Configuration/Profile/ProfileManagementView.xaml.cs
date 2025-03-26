using System.Windows.Controls;
using StroopApp.ViewModels;
using StroopApp.Services.Profile;

namespace StroopApp.Views.Profile
{
    public partial class ProfileManagementView : UserControl
    {
        // Constructeur sans paramètre pour le XAML
        public ProfileManagementView() : this(new ProfileManagementViewModel(new ProfileService()))
        {
        }

        // Constructeur avec injection du ViewModel
        public ProfileManagementView(ProfileManagementViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
