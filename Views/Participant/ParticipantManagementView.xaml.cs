using System.Windows.Controls;
using StroopApp.ViewModels;
using StroopApp.Services.Participant;

namespace StroopApp.Views.Participant
{
    public partial class ParticipantManagementView : UserControl
    {
        // Constructeur sans paramètre pour le XAML
        public ParticipantManagementView() : this(new ParticipantManagementViewModel(new ParticipantService()))
        {
        }

        // Constructeur avec injection du ViewModel
        public ParticipantManagementView(ParticipantManagementViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
