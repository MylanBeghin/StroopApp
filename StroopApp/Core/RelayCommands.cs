using System.Windows.Input;

namespace StroopApp.Core
{
    /// <summary>
    /// A generic command implementation of <see cref="ICommand"/> that delegates execution and can-execute logic.
    /// Supports commands with or without parameters.
    /// </summary>

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        public RelayCommand(Action execute, Func<bool> canExecute = null)
            : this(o => execute(), o => canExecute == null || canExecute())
        {
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
    /// <summary>
    /// A strongly-typed generic implementation of <see cref="ICommand"/> for commands using a parameter of type <typeparamref name="T"/>.
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        public RelayCommand(Action execute, Func<bool> canExecute = null)
            : this(o => execute(), o => canExecute == null || canExecute())
        {
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;
            if (parameter == null && typeof(T).IsValueType)
                return _canExecute(default);
            if (parameter == null)
                return _canExecute(default!);
            return _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            if (parameter == null && typeof(T).IsValueType)
                _execute(default);
            else if (parameter == null)
                _execute(default!);
            else
                _execute((T)parameter);
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
