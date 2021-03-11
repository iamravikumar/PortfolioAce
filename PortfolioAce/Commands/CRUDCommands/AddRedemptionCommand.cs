using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                if (_investorActionVM.Units < decimal.One)
                {
                    throw new InvalidOperationException("The redemption units must be greater than or equal 1");
                }
                
                TransferAgencyBO newInvestorAction = new TransferAgencyBO
                {
                    TransactionDate = _investorActionVM.TradeDate,
                    TransactionSettleDate = _investorActionVM.TradeDate,
                    IssueType = _investorActionVM.TAType,
                    Units = _investorActionVM.Units *-1,
                    NAVPrice = decimal.Zero,
                    TradeAmount = decimal.Zero,
                    Currency = _investorActionVM.Currency,
                    Fees = _investorActionVM.Fee,
                    FundId = _investorActionVM.FundId,
                    IsNavFinal = false
                };
                FundInvestorBO fundInvestor = _investorService.GetFundInvestor(_investorActionVM.FundId, _investorActionVM.SelectedInvestor.InvestorId);
                if (fundInvestor == null)
                {
                    throw new InvalidOperationException($"{_investorActionVM.SelectedInvestor.FullName} does not have any shares to redeem");
                }
                else
                {
                    newInvestorAction.FundInvestorId = fundInvestor.FundInvestorId;
                }
                await _investorService.CreateInvestorAction(newInvestorAction);
                _investorActionVM.CloseAction();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
