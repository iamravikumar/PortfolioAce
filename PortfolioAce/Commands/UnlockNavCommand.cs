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
    public class UnlockNavCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private NavSummaryViewModel _navValuationVM;
        private ITransferAgencyService _investorService;

        public UnlockNavCommand(NavSummaryViewModel navValuationVM, ITransferAgencyService investorService)
        {
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
                string message = "Are you sure that you want to unlock this period? This action is not reversible.";
                MessageBoxResult result = MessageBox.Show(message, "Unlock Period", button: MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        _investorService.UnlockNav(_navValuationVM.AsOfDate, _navValuationVM.FundId);
                        _navValuationVM.CloseAction();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
