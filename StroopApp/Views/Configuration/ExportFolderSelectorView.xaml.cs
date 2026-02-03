using StroopApp.ViewModels.Configuration;
using System.Windows.Controls;

namespace StroopApp.Views.Configuration
{
    public partial class ExportFolderSelectorView : UserControl
    {
        public ExportFolderSelectorView(ExportFolderSelectorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
