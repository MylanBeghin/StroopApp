using StroopApp.Models;
using StroopApp.ViewModels.Configuration;
using StroopApp.ViewModels.Experiment;
using StroopApp.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StroopApp.ViewModels
{
    public class ExperimentWindowViewModel : INotifyPropertyChanged
    {
        
        public ExperimentSettings Settings { get; private set; }

        private readonly ConfigurationPageViewModel _configurationPageViewModel;

        private readonly ExperimentDashBoardPageViewModel _experimentWindowViewModel;
        public ExperimentWindowViewModel(ConfigurationPage configPage, ExperimentDashBoardPage experimentPage)
        {
            _configurationPageViewModel = (ConfigurationPageViewModel)configPage.DataContext;
            _experimentWindowViewModel = (ExperimentDashBoardPageViewModel)experimentPage.DataContext;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
