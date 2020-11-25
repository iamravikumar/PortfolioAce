using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.FactTables;
using PortfolioAce.EFCore.Services;
using PortfolioAce.HelperObjects;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
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
                            Units = seedInvestor.SeedAmount / _fundInitialiseVM.NavPrice,
                            IsNavFinal=true,
                        };
                        newInvestors.Add(newInvestor);
                    }
                    else
                    {
                        throw new ArgumentException("Investor must have a name and the seed amount must be greater than 0");
                    }
                }
                NAVPriceStoreFACT initialNav = new NAVPriceStoreFACT
                {
                    FinalisedDate = updateFund.LaunchDate,
                    NAVPrice = _fundInitialiseVM.NavPrice,
                    FundId = updateFund.FundId,
                    NetAssetValue=newInvestors.Sum(ni=>ni.TradeAmount), 
                    SharesOutstanding=newInvestors.Sum(ni=>ni.Units) ,
                    Currency = updateFund.BaseCurrency
                };
                _investorService.InitialiseFundAction(updateFund, newInvestors, initialNav);// takes in
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
