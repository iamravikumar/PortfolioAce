using PortfolioAce.Domain.DataObjects;
using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class LockNavCommand : AsyncCommandBase
    {
        private NavValuations _navValuation;
        private NavSummaryViewModel _navValuationVM;
        private ITransferAgencyService _investorService;

        public LockNavCommand(NavSummaryViewModel navValuationVM, NavValuations navValuation, ITransferAgencyService investorService)
        {
            _navValuation = navValuation;
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
                await _investorService.LockNav(_navValuation);
                _navValuationVM.CloseAction();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
