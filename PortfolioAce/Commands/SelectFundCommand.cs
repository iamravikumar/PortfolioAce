using PortfolioAce.EFCore.Services;
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
        private IFundRepository _fundRepo;
        public SelectFundCommand(AllFundsViewModel allFundsViewModel, IFundRepository fundRepo)
        {
            _allFundsWindowVM = allFundsViewModel;
            _fundRepo = fundRepo;
        }

        public bool CanExecute(object parameter)
        {
            return true; //For now
        }

        public void Execute(object parameter)
        {
            string fundSymbol = (string)parameter;
            _allFundsWindowVM.CurrentFund = _fundRepo.GetFund(fundSymbol);
        }
    }
}
