using System;
using System.Windows.Input;

namespace Presentation.ModelView
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action execute) : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (() => true); // Domyślnie można zawsze wykonać
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute();
        }

        public void Execute(object? parameter)
        {
            _execute();
        }

        // Powiadomienie o zmianach w stanie 'CanExecute'
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}