using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class UnlockNavCommand : AsyncCommandBase
    {
        private NavSummaryViewModel _navValuationVM;
        private ITransferAgencyService _investorService;

        public UnlockNavCommand(NavSummaryViewModel navValuationVM, ITransferAgencyService investorService)
        {
            _investorService = investorService;
            _navValuationVM = navValuationVM;
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }


        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                string message = "Are you sure that you want to unlock this period? This action is not reversible.";
                MessageBoxResult result = MessageBox.Show(message, "Unlock Period", button: MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        if (_navValuationVM.AsOfDate == _navValuationVM.FundLaunchDate)
                        {
                            throw new InvalidOperationException("You cannot unlock the initial period.");
                        }
                        else
                        {
                            await _investorService.UnlockNav(_navValuationVM.AsOfDate, _navValuationVM.FundId);
                            _navValuationVM.CloseAction();
                        }
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
