using StroopApp.Services.Profile;
using System.Windows;

namespace StroopApp.Views
{
    public partial class ExperimentWindow : Window
    {
        public ExperimentWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new ConfigurationPage());
        }
    }
}
