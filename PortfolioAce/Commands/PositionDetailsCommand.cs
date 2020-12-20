using PortfolioAce.Domain.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class PositionDetailsCommand:ICommand
    {
        public event EventHandler CanExecuteChanged;

        public PositionDetailsCommand()
        {
        }

        public bool CanExecute(object parameter)
        {
            return true; //For now
        }

        public void Execute(object parameter)
        {
            // This will open a window with extra breakdown details...
            var x = (SecurityPositionValuation)parameter;
            MessageBox.Show($"{x.Position.security.SecurityName}: {x.Position.NetQuantity} ");
        }
    }
}
