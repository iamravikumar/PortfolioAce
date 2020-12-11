using PortfolioAce.Domain.DataObjects;
using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class LockNavCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private NavValuations _navValuation;
        private ITransferAgencyService _investorService;

        public LockNavCommand(NavValuations navValuation, ITransferAgencyService investorService)
        {
            _navValuation = navValuation;
            _investorService = investorService;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine("Pending");
        }
    }
}
