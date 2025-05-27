using StroopApp.ViewModels.Configuration;
using System.Windows.Controls;

namespace StroopApp.Views.Configuration
{
    /// <summary>
    /// Logique d'interaction pour ExportFolderSelectorView.xaml
    /// </summary>
    public partial class ExportFolderSelectorView : UserControl
    {
        public ExportFolderSelectorView(ExportFolderSelectorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
