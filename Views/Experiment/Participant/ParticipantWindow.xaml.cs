using System.Collections.Generic;
using System.Windows;
using StroopApp.Models;
namespace StroopApp.Views
{
    public partial class ParticipantWindow : Window
    {
        public ParticipantWindow(ExperimentSettings Settings)
        {
            InitializeComponent();
            DataContext = Settings;
            MainFrame.Navigate(new InstructionsPage(Settings.CurrentProfile.StroopType));
        }
    }
}
