using PortfolioAce.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
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

    public class ActionCommand<T1, T2, T3> : ICommand
    {
        private readonly Action<T1, T2, T3> _execute;
        private readonly Func<bool> _canExecute;
        //private readonly Action<Window, ViewModelWindowBase> _executeParams = null;
        private T1 arg1;
        private T2 arg2;
        private T3 arg3;

        public ActionCommand(Action<T1, T2, T3> execute, T1 a, T2 b, T3 c) : this(execute, () => true)
        {
            arg1 = a;
            arg2 = b;
            arg3 = c;
        }

        public ActionCommand(Action<T1, T2, T3> execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute.Invoke();

        public void Execute(object parameter)
        {
            _execute.Invoke(arg1,arg2, arg3);
        }
    }

}
