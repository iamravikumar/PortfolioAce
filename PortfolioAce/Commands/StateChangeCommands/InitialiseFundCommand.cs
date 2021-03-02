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
using System.Windows;
using System.Windows.Input;

namespace PortfolioAce.Commands
{
    public class InitialiseFundCommand : ICommand
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
                if (_fundInitialiseVM.dgSeedingInvestors.Count == 0)
                {
                    throw new ArgumentException("Your fund must have initial investors");
                }

                Fund updateFund = _fundInitialiseVM.TargetFund;
                updateFund.IsInitialised = true;

                string cashSymbol = $"{updateFund.BaseCurrency}c";
                SecuritiesDIM security = _staticReferences.GetSecurityInfo(cashSymbol);
                TransactionTypeDIM tradeType = _staticReferences.GetTransactionType("Deposit");
                CustodiansDIM custodian = _staticReferences.GetCustodian(_fundInitialiseVM.Custodian);

                List<TransferAgencyBO> subscriptions = new List<TransferAgencyBO>();
                List<TransactionsBO> transactions = new List<TransactionsBO>();
                List<FundInvestorBO> fundInvestors = new List<FundInvestorBO>();
                List<InvestorHoldingsFACT> investorHoldings = new List<InvestorHoldingsFACT>();

                foreach (SeedingInvestor seedInvestor in _fundInitialiseVM.dgSeedingInvestors)
                {
                    if (seedInvestor.SeedAmount >= updateFund.MinimumInvestment)
                    {
                        FundInvestorBO fundInvestor = new FundInvestorBO
                        {
                            InceptionDate = updateFund.LaunchDate,
                            FundId = updateFund.FundId,
                            InvestorId = seedInvestor.InvestorId
                        };
                        // The highwatermark is only applicable if the fund has a highwatermark...
                        fundInvestor.HighWaterMark = (updateFund.HasHighWaterMark) ? _fundInitialiseVM.NavPrice : (decimal?)null;
                        fundInvestors.Add(fundInvestor);

                        InvestorHoldingsFACT investor = new InvestorHoldingsFACT
                        {
                            ManagementFeesAccrued = decimal.Zero,
                            PerformanceFeesAccrued = decimal.Zero,
                            FundId = updateFund.FundId,
                            HoldingDate = updateFund.LaunchDate,
                            InvestorId = seedInvestor.InvestorId,
                            AverageCost = _fundInitialiseVM.NavPrice,
                            Units = seedInvestor.SeedAmount / _fundInitialiseVM.NavPrice,
                            NetValuation = seedInvestor.SeedAmount,
                        };
                        investor.HighWaterMark = (updateFund.HasHighWaterMark) ? _fundInitialiseVM.NavPrice : (decimal?)null;
                        investorHoldings.Add(investor);
                        // hwm
                        TransferAgencyBO newSubscription = new TransferAgencyBO
                        {
                            TradeAmount = seedInvestor.SeedAmount,
                            NAVPrice = _fundInitialiseVM.NavPrice,
                            TransactionDate = updateFund.LaunchDate,
                            TransactionSettleDate = updateFund.LaunchDate,
                            Currency = updateFund.BaseCurrency,
                            FundId = updateFund.FundId,
                            Fees = 0,
                            IssueType = "Subscription",
                            Units = seedInvestor.SeedAmount / _fundInitialiseVM.NavPrice,
                            IsNavFinal = true,
                            FundInvestor = fundInvestor
                        };
                        subscriptions.Add(newSubscription);
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
                            isCashTransaction = false,
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
                        throw new ArgumentException("The seed amount must be greater than the Funds minimum investment");
                    }
                }

                int PeriodId = _staticReferences.GetPeriod(updateFund.LaunchDate, updateFund.FundId).PeriodId;
                NAVPriceStoreFACT initialNav = new NAVPriceStoreFACT
                {
                    FinalisedDate = updateFund.LaunchDate,
                    NAVPrice = _fundInitialiseVM.NavPrice,
                    FundId = updateFund.FundId,
                    NetAssetValue = subscriptions.Sum(ni => ni.TradeAmount),
                    SharesOutstanding = subscriptions.Sum(ni => ni.Units),
                    Currency = updateFund.BaseCurrency,
                    NAVPeriodId = PeriodId
                };
                _investorService.InitialiseFundAction(updateFund, subscriptions, transactions, initialNav, fundInvestors, investorHoldings);
                _fundInitialiseVM.CloseAction();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
