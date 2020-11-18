using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using PortfolioAce.HelperObjects;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class InitialiseFundCommand:ICommand
    {
        public event EventHandler CanExecuteChanged;

        private FundInitialisationWindowViewModel _fundInitialiseVM;
        private ITransferAgencyService _investorService;

        public InitialiseFundCommand(FundInitialisationWindowViewModel fundInitialiseVM,
            ITransferAgencyService investorService)
        {
            _fundInitialiseVM = fundInitialiseVM;
            _investorService = investorService;
        }

        public bool CanExecute(object parameter)
        {
            return true; // true for now
        }

        public async void Execute(object parameter)
        {
            try
            {
                Fund updateFund = _fundInitialiseVM.TargetFund;
                updateFund.IsInitialised = true;
                List<TransferAgency> newInvestors = new List<TransferAgency>();
                foreach(SeedingInvestor seedInvestor in _fundInitialiseVM.dgSeedingInvestors)
                {
                    if (seedInvestor.InvestorName!="" || seedInvestor.SeedAmount>0 ) 
                    {
                        TransferAgency newInvestor = new TransferAgency
                        {
                            InvestorName = seedInvestor.InvestorName,
                            TradeAmount = seedInvestor.SeedAmount,
                            NAVPrice = _fundInitialiseVM.NavPrice,
                            TransactionDate = updateFund.LaunchDate,
                            TransactionSettleDate = updateFund.LaunchDate,
                            Currency = updateFund.BaseCurrency,
                            FundId = updateFund.FundId,
                            Fees = 0,
                            Type = "Subscription",
                            Units = seedInvestor.SeedAmount / _fundInitialiseVM.NavPrice
                        };
                        newInvestors.Add(newInvestor);
                    }
                    else
                    {
                        throw new ArgumentException("Investor must have a name and the seed amount must be greater than 0");
                    }
                }
                _investorService.InitialiseFundAction(updateFund, newInvestors);// takes in
                //await _investorService.CreateInvestorAction(newInvestorAction);
                _fundInitialiseVM.CloseAction();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
