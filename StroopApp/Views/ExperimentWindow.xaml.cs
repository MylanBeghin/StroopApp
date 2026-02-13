using StroopApp.Services.Language;
using StroopApp.Services.Navigation.PageFactory;
using StroopApp.ViewModels;
using System.Windows;

namespace StroopApp.Views
{
    public partial class ExperimentWindow : Window
    {
        public ExperimentWindow(IPageFactory pageFactory, ILanguageService languageService)
        {
            InitializeComponent();
            var navigationService = new NavigationService(pageFactory);
            navigationService.SetFrame(MainFrame);
            DataContext = new ExperimentWindowViewModel(navigationService, languageService);
        }
    }
}
