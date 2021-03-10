using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.Commands.CRUDCommands
{
    public class AddRedemptionCommand : AsyncCommandBase
    {
        private InvestorActionViewModel _investorActionVM;
        private ITransferAgencyService _investorService;

        public AddRedemptionCommand(InvestorActionViewModel investorActionVM,
            ITransferAgencyService investorService)
        {
            _investorActionVM = investorActionVM;
            _investorService = investorService;
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {

            }
            catch (Exception e)
            {
            }
        }
    }
}
