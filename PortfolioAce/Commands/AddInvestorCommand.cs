using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class AddInvestorCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private AddInvestorWindowViewModel _addInvestorVM;
        private ITransferAgencyService _investorService;

        public AddInvestorCommand(AddInvestorWindowViewModel addInvestorVM,
    ITransferAgencyService investorService)
        {
            _addInvestorVM = addInvestorVM;
            _investorService = investorService;
        }

        public bool CanExecute(object parameter)
        {
            return true; // true for now
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
