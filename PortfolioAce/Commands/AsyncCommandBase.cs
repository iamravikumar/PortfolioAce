using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public abstract class AsyncCommandBase : ICommand
    {

        private bool _isExecuting; //ensures the command cannot be spammed.
        public bool IsExecuting
        {
            get
            {
                return _isExecuting;
            }
            set
            {
                _isExecuting = value;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter)
        {
            return !IsExecuting;
        }

        public async void Execute(object parameter)
        {
            IsExecuting = true;
            await ExecuteAsync(parameter);
            IsExecuting = false;
        }

        public abstract Task ExecuteAsync(object parameter);
    }
}
