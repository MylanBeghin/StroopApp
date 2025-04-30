using StroopApp.Models;
using StroopApp.Services.Window;
using StroopApp.ViewModels;
using System.Windows;

namespace StroopApp.Views
{
    public partial class ExperimentWindow : Window
    {
        public ExperimentWindow(ExperimentSettings settings, IWindowManager windowManager)
        {
            InitializeComponent();
            DataContext = new ExperimentWindowViewModel(settings, new NavigationService(MainFrame), windowManager);
        }
    }
}
