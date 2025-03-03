using System;
using System.Windows.Input;

namespace StroopApp.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // L'événement qui notifie les modifications de la capacité d'exécuter la commande
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        // Vérifie si la commande peut être exécutée
        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        // Exécute la commande
        public void Execute(object? parameter) => _execute();
    }
}
