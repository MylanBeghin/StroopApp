namespace StroopApp.Views
{
    public partial class ExperimentWindow
    {
        public ExperimentWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new ConfigurationPage());
        }
    }
}
