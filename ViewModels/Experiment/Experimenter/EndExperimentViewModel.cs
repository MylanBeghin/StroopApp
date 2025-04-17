using System.Runtime.CompilerServices;
using System.ComponentModel;
using StroopApp.Models;
using System.Windows.Input;
using StroopApp.Commands;

namespace StroopApp.ViewModels.Experiment.Experimenter
{
    public class EndExperimentViewModel : INotifyPropertyChanged
    {
        private ExperimentSettings _settings;
        public ExperimentSettings Settings
        {
            get => _settings;
            set { _settings = value; }
        }
        public ICommand QuitCommand { get; }
        public ICommand RestartCommand { get; }
        public EndExperimentViewModel(ExperimentSettings settings)
        {
            Settings = settings;
            QuitCommand = new RelayCommand(Quit);
            RestartCommand = new RelayCommand(Restart);
        }
        private void Quit()
        {
        }
        private void Restart()
        {
            // Logic to restart the experiment
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
