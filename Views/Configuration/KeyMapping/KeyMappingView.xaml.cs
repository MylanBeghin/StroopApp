using StroopApp.ViewModels.Configuration;
using System.Windows.Controls;

namespace StroopApp.Views.KeyMapping
{
    public partial class KeyMappingView : UserControl
    {
        public KeyMappingView(KeyMappingViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
