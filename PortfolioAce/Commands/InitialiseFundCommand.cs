using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.Domain.Models.FactTables;
using PortfolioAce.EFCore.Services;
using PortfolioAce.EFCore.Services.DimensionServices;
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
        private IStaticReferences _staticReferences;

        public InitialiseFundCommand(FundInitialisationWindowViewModel fundInitialiseVM,
            ITransferAgencyService investorService, IStaticReferences staticReferences)
        {
            _fundInitialiseVM = fundInitialiseVM;
            _investorService = investorService;
            _staticReferences = staticReferences;
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

                string cashSymbol = $"{updateFund.BaseCurrency}c";
                SecuritiesDIM security = _staticReferences.GetSecurityInfo(cashSymbol);
                TransactionTypeDIM tradeType = _staticReferences.GetTransactionType("Deposit");
                CustodiansDIM custodian = _staticReferences.GetCustodian("Default");

                List<TransferAgencyBO> newInvestors = new List<TransferAgencyBO>();
                List<TransactionsBO> transactions = new List<TransactionsBO>();
                foreach(SeedingInvestor seedInvestor in _fundInitialiseVM.dgSeedingInvestors)
                {
                    if (seedInvestor.InvestorName!="" || seedInvestor.SeedAmount>0 ) 
                    {
                        TransferAgencyBO newInvestor = new TransferAgencyBO
                        {
                            InvestorName = seedInvestor.InvestorName,
                            TradeAmount = seedInvestor.SeedAmount,
                            NAVPrice = _fundInitialiseVM.NavPrice,
                            TransactionDate = updateFund.LaunchDate,
                            TransactionSettleDate = updateFund.LaunchDate,
                            Currency = updateFund.BaseCurrency,
                            FundId = updateFund.FundId,
                            Fees = 0,
                            IssueType = "Subscription",
                            Units = seedInvestor.SeedAmount / _fundInitialiseVM.NavPrice,
                            IsNavFinal=true,
                        };
                        newInvestors.Add(newInvestor);
                        TransactionsBO newTransaction = new TransactionsBO
                        {
                            SecurityId = security.SecurityId,
                            Quantity = seedInvestor.SeedAmount,
                            Price = decimal.One,
                            TradeAmount = seedInvestor.SeedAmount,
                            TradeDate = updateFund.LaunchDate,
                            SettleDate = updateFund.LaunchDate,
                            CreatedDate = DateTime.Now,
                            LastModified = DateTime.Now,
                            Fees = decimal.Zero,
                            isActive = true,
                            isLocked = true,
                            FundId = updateFund.FundId,
                            TransactionTypeId = tradeType.TransactionTypeId,
                            CurrencyId = security.CurrencyId,
                            Comment = "Initial Subscription",
                            CustodianId = custodian.CustodianId
                        };
                        transactions.Add(newTransaction);

                    }
                    else
                    {
                        throw new ArgumentException("Investor must have a name and the seed amount must be greater than 0");
                    }
                }

                int PeriodId = _staticReferences.GetPeriod(updateFund.LaunchDate, updateFund.FundId).PeriodId;
                NAVPriceStoreFACT initialNav = new NAVPriceStoreFACT
                {
                    FinalisedDate = updateFund.LaunchDate,
                    NAVPrice = _fundInitialiseVM.NavPrice,
                    FundId = updateFund.FundId,
                    NetAssetValue = newInvestors.Sum(ni => ni.TradeAmount),
                    SharesOutstanding = newInvestors.Sum(ni => ni.Units),
                    Currency = updateFund.BaseCurrency,
                    NAVPeriodId = PeriodId
                };
                _investorService.InitialiseFundAction(updateFund, newInvestors,transactions, initialNav);
                _fundInitialiseVM.CloseAction();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
