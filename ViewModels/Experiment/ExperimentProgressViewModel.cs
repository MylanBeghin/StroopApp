using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StroopApp.Models;

namespace StroopApp.ViewModels.Experiment
{
    public class ExperimentProgressViewModel : INotifyPropertyChanged
    {
        
        public ExperimentProgressViewModel(ExperimentProfile Settings)
        {
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
