using System.Windows;

using StroopApp.ViewModels.Configuration;

namespace StroopApp.Views.Configuration
{
	/// <summary>
	/// Logique d'interaction pour SerialSettingsWindow.xaml
	/// </summary>
	public partial class AdvancedSettingsWindow : Window
	{
		public AdvancedSettingsWindow()
		{
			InitializeComponent();
			DataContext = new AdvancedSettingsWindowViewModel();
		}
	}
}
