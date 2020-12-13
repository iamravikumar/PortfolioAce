using PortfolioAce.Domain.DataObjects;
using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class LockNavCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private NavValuations _navValuation;
        private NavSummaryViewModel _navValuationVM;
        private ITransferAgencyService _investorService;

        public LockNavCommand(NavSummaryViewModel navValuationVM, NavValuations navValuation, ITransferAgencyService investorService)
        {
            _navValuation = navValuation;
            _investorService = investorService;
            _navValuationVM = navValuationVM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                // if the fund is monthly and the period to lock does not match the periods in the database prevent the lock operation this...
                _investorService.LockNav(_navValuation);
                _navValuationVM.CloseAction();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
