using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.Views;
using System.Windows;

namespace StroopApp
{
    public partial class App : Application
    {
        public static IWindowManager WindowManager { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var settings = new ExperimentSettings();
            WindowManager = new WindowManager();
            var expWin = new ExperimentWindow(settings, WindowManager);
            expWin.Show();
        }
    }
}
