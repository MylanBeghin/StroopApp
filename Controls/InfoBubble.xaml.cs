using System.Windows;
using System.Windows.Controls;

namespace StroopApp.Controls
{
    public partial class InfoBubble : UserControl
    {
        public InfoBubble() => InitializeComponent();

        public static readonly DependencyProperty InfoTextProperty =
            DependencyProperty.Register(
                nameof(InfoText),
                typeof(string),
                typeof(InfoBubble),
                new PropertyMetadata(string.Empty));

        public string InfoText
        {
            get => (string)GetValue(InfoTextProperty);
            set => SetValue(InfoTextProperty, value);
        }
    }
}
