using StroopApp.ViewModels.Configuration.Participant;
using System.Windows;

namespace StroopApp.Views.Participant
{
    public partial class ParticipantEditorWindow : Window
    {
        public ParticipantEditorWindow(ParticipantEditorViewModel viewModel)
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
