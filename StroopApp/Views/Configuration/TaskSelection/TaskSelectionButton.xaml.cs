using System.Windows;
using System.Windows.Controls;

namespace StroopApp.Views.Configuration.TaskSelection
{
    public partial class TaskSelectionButton : UserControl
    {

        public static readonly DependencyProperty TextTitleProperty =
        DependencyProperty.Register("TextTitle", typeof(string), typeof(TaskSelectionButton));

        public static readonly DependencyProperty TextDescriptionProperty =
            DependencyProperty.Register("TextDescription", typeof(string), typeof(TaskSelectionButton));

        public string TextTitle
        {
            get => (string)GetValue(TextTitleProperty);
            set => SetValue(TextTitleProperty, value);
        }

        public string TextDescription
        {
            get => (string)GetValue(TextDescriptionProperty);
            set => SetValue(TextDescriptionProperty, value);
        }

        public TaskSelectionButton()
        {
            InitializeComponent();
        }

    }
}
