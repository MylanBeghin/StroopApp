using System.Globalization;
using System.Windows;

using StroopApp.Models;
using StroopApp.Services.Language;
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
		public static ILanguageService LanguageService { get; private set; }
		protected override void OnStartup(StartupEventArgs e)
		{
			LanguageService = new LanguageService();
			base.OnStartup(e);
			var settings = new ExperimentSettings();
			WindowManager = new WindowManager();
			var expWin = new ExperimentWindow(settings, WindowManager, LanguageService);
			expWin.Show();
		}
	}
}
