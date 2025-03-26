using StroopApp.ViewModels.Configuration;
using StroopApp.ViewModels.Experiment;
using StroopApp.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StroopApp.ViewModels
{
    public class ExperimentWindowViewModel : INotifyPropertyChanged
    {
        private int block;

        public int Block
        {
            get => block;
            set
            {
                if (value != block)
                {
                    block = value;
                    OnPropertyChanged();
                }
                    
            }
        }
        private readonly ConfigurationPageViewModel _configurationPageViewModel;

        private readonly ExperimentDashBoardPageViewModel _experimentWindowViewModel;
        public ExperimentWindowViewModel()
        {
            Block = 0;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
