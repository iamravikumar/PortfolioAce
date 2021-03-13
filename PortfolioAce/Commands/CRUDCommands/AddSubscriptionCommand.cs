using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class AddSubscriptionCommand : AsyncCommandBase
    {
        private InvestorActionViewModel _investorActionVM;
        private ITransferAgencyService _investorService;

        public AddSubscriptionCommand(InvestorActionViewModel investorActionVM,
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
                if (_investorActionVM.TradeAmount <= _investorActionVM.TargetFundMinimumInvestment)
                {
                    throw new ArgumentException($"The Subscription amount must be greater than the Funds minimum investment: {_investorActionVM.TargetFundMinimumInvestment} {_investorActionVM.TargetFundBaseCurrency}.");
                }
                //For now settledate = trade date TODO soon it will be td + fund subscription date
                TransferAgencyBO newInvestorAction = new TransferAgencyBO
                {
                    TransactionDate = _investorActionVM.TradeDate,
                    TransactionSettleDate = _investorActionVM.TradeDate,
                    IssueType = _investorActionVM.TAType,
                    Units = decimal.Zero,
                    NAVPrice = decimal.Zero,
                    TradeAmount = _investorActionVM.TradeAmount,
                    Currency = _investorActionVM.Currency,
                    Fees = _investorActionVM.Fee,
                    FundId = _investorActionVM.FundId,
                    IsNavFinal = false
                };
                FundInvestorBO fundInvestor = _investorService.GetFundInvestor(_investorActionVM.FundId, _investorActionVM.SelectedInvestor.InvestorId);
                if (fundInvestor == null)
                {
                    //this means the investor is new to the fund.
                    fundInvestor = new FundInvestorBO
                    {
                        InceptionDate = _investorActionVM.TradeDate,
                        FundId = _investorActionVM.FundId,
                        InvestorId = _investorActionVM.SelectedInvestor.InvestorId,
                    };
                    fundInvestor.HighWaterMark = (_investorActionVM.TargetFundWaterMark && _investorActionVM.isNavFinal) ? _investorActionVM.Price : (decimal?)null;
                    newInvestorAction.FundInvestor = fundInvestor;
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
