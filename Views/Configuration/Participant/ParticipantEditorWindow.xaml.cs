using System.Windows;
using StroopApp.ViewModels;

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
