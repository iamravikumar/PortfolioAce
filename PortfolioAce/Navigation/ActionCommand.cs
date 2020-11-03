using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.Navigation
{
    public class ActionCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public ActionCommand(Action execute) : this(execute, () => true)
        {
        }

        public ActionCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute.Invoke();

        public void Execute(object parameter)
        {
            _execute.Invoke();
        }
    }

}
