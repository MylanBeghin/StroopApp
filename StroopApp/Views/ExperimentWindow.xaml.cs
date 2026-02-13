using StroopApp.Models;
using StroopApp.Services.Language;
using StroopApp.Services.Navigation.PageFactory;
using StroopApp.Services.Window;
using StroopApp.ViewModels;
using System.Windows;

namespace StroopApp.Views
{
    public partial class ExperimentWindow : Window
    {
        public ExperimentWindow(ExperimentSettings settings, IPageFactory pageFactory, IWindowManager windowManager, ILanguageService languageService)
        {
            InitializeComponent();
            var navigationService = new NavigationService(pageFactory);
            navigationService.SetFrame(MainFrame);
            DataContext = new ExperimentWindowViewModel(settings, navigationService, languageService);
        }
    }
}
