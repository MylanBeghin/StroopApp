using StroopApp.Services.Navigation.PageFactory;
using StroopApp.Views.Home;
using System.Windows;

namespace StroopApp.Views
{
    /// <summary>
    /// Logique d'interaction pour SImonExperimentWindow.xaml
    /// </summary>
    public partial class SimonExperimentWindow : Window
    {
        public SimonExperimentWindow(IPageFactory pageFactory)
        {
            InitializeComponent();
            var navigationService = new NavigationService(pageFactory);
            navigationService.SetFrame(SimonExperimentFrame);
            navigationService.NavigateTo<HomePage>();
        }
    }
}
