using System.Windows;

using StroopApp.Models;
using StroopApp.Services.Language;
using StroopApp.Services.Window;
using StroopApp.ViewModels;

namespace StroopApp.Views
{
	public partial class ExperimentWindow : Window
	{
		public ExperimentWindow(ExperimentSettings settings, IWindowManager windowManager, ILanguageService languageService)
		{
			InitializeComponent();
			DataContext = new ExperimentWindowViewModel(settings, new NavigationService(MainFrame), windowManager, languageService);
		}
	}
}
