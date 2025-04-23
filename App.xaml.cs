using System.Windows;
using StroopApp.Models;
using StroopApp.Services.Exportation;
using StroopApp.Services.Navigation;
using StroopApp.Views;

namespace StroopApp
{
    public partial class App : Application
    {
        public static INavigationService ExperimentWindowNavigationService { get; set; }
        public static INavigationService ParticipantWindowNavigationService { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var settings = new ExperimentSettings();
            var expWin = new ExperimentWindow(settings);
            expWin.Show();
        }
    }
}
