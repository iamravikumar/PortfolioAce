using PortfolioAce.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class SelectFundCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private AllFundsViewModel _allFundsWindowVM;
        public SelectFundCommand(AllFundsViewModel allFundsViewModel)
        {
            _allFundsWindowVM = allFundsViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true; //For now
        }

        public void Execute(object parameter)
        {

            Console.WriteLine(parameter);
            _allFundsWindowVM.CurrentFund = (string)parameter;
        }
    }
}
