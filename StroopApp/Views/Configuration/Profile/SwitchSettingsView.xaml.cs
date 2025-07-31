using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace StroopApp.Views.Configuration.Profile
{
	public partial class SwitchSettingsView : UserControl
	{
		public SwitchSettingsView()
		{
			InitializeComponent();
		}
		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			// Ouvre le lien dans le navigateur par défaut
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
			e.Handled = true;
		}
	}
}
