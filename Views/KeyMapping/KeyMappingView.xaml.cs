using ModernWpf.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StroopApp.ViewModels;
using StroopApp.Services.KeyMapping;
using StroopApp.Services.Profile;

namespace StroopApp.Views.KeyMapping
{
    public partial class KeyMappingView : UserControl
    {
        public KeyMappingView()
        {
            KeyMappingService keyMappingService = new KeyMappingService();
            InitializeComponent();
            DataContext = new KeyMappingViewModel(keyMappingService);
        }

        
    }
}
