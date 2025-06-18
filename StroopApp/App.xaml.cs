using System.Globalization;
using System.Windows;

using StroopApp.Models;
using StroopApp.Services.Window;
using StroopApp.Views;

namespace StroopApp
{
	public partial class App : Application
	{
		public static IWindowManager WindowManager
		{
			get; private set;
		}
		protected override void OnStartup(StartupEventArgs e)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
			base.OnStartup(e);
			var settings = new ExperimentSettings();
			WindowManager = new WindowManager();
			var expWin = new ExperimentWindow(settings, WindowManager);
			expWin.Show();
		}
	}
}
