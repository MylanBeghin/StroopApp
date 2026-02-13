using StroopApp.Models;
using StroopApp.Services.Language;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.ViewModels;
using System.Windows;
using System.Windows.Navigation;

namespace StroopApp.Views
{
    public partial class ExperimentWindow : Window
    {
        public ExperimentWindow(ExperimentSettings settings,INavigationService navigationService, IWindowManager windowManager, ILanguageService languageService)
        {
            InitializeComponent();
            (navigationService).SetFrame(MainFrame);
            DataContext = new ExperimentWindowViewModel(settings, navigationService, windowManager, languageService);
        }
    }
}
