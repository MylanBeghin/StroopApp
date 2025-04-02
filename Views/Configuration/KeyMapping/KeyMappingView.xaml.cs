using System.Windows.Controls;
using StroopApp.Services.KeyMapping;
using StroopApp.ViewModels.Configuration;

namespace StroopApp.Views.KeyMapping
{
    public partial class KeyMappingView : UserControl
    {
        // Constructeur sans paramètre pour le XAML
        public KeyMappingView() : this(new KeyMappingViewModel(new KeyMappingService()))
        {
        }

        // Constructeur avec injection du ViewModel
        public KeyMappingView(KeyMappingViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
