using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.DataObjects.PositionData;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.Domain.Models.FactTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public class TransferAgencyService : ITransferAgencyService
    {

        private readonly PortfolioAceDbContextFactory _contextFactory;

        public TransferAgencyService(PortfolioAceDbContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }

        public async Task<InvestorsDIM> CreateInvestor(InvestorsDIM investor)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<InvestorsDIM> res = await context.Investors.AddAsync(investor);
                await context.SaveChangesAsync();

                return res.Entity;
            }
        }

        public async Task<TransferAgencyBO> CreateInvestorAction(TransferAgencyBO investorAction)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<TransferAgencyBO> res = await context.TransferAgent.AddAsync(investorAction);
                await context.SaveChangesAsync();

                return res.Entity;
            }
        }

        public async Task<TransferAgencyBO> DeleteInvestorAction(int id)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                TransferAgencyBO investorAction = await context.TransferAgent.FindAsync(id);
                if (investorAction == null)
                {
                    return investorAction;
                }
                context.TransferAgent.Remove(investorAction);
                await context.SaveChangesAsync();

                return investorAction;
            }
        }

        public List<TransferAgencyBO> GetAllFundInvestorActions(int fundId)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.TransferAgent.Where(ta => ta.FundId == fundId).OrderBy(ta => ta.TransactionDate).ToList();
            }
        }

        public List<FundInvestorBO> GetAllFundInvestors(int fundId)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.FundInvestor.Where(fi => fi.FundId == fundId).ToList();
            }
        }

        public List<InvestorsDIM> GetAllInvestors()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Investors.ToList();
            }
        }

        public FundInvestorBO GetFundInvestor(int fundId, int investorId)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.FundInvestor.Where(fi => fi.FundId == fundId && fi.InvestorId == investorId).FirstOrDefault();
            }
        }

        public async Task<TransferAgencyBO> GetInvestorActionById(int id)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.TransferAgent.FindAsync(id);
            }
        }

        public async Task InitialiseFundAction(Fund fund, List<TransferAgencyBO> investorSubscriptions, List<TransactionsBO> transactions, NAVPriceStoreFACT initialNav, List<FundInvestorBO> fundInvestors, List<InvestorHoldingsFACT> investorHoldings)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                // I will need to create a positions Fact table AND a investor holdings here too.
                // position is easy sum all the transactions together...
                decimal cashBalance = transactions.Sum(s => s.TradeAmount);
                int assetClassId = context.Securities.Find(transactions[0].SecurityId).AssetClassId;
                PositionFACT cashPosition = new PositionFACT
                {
                    PositionDate = fund.LaunchDate,
                    AssetClassId = assetClassId,
                    AverageCost = decimal.One,
                    CurrencyId = transactions[0].CurrencyId,
                    UnrealisedPnl = decimal.Zero,
                    FundId = fund.FundId,
                    MarketValue = cashBalance,
                    Price = decimal.One,
                    Quantity = cashBalance,
                    RealisedPnl = decimal.Zero,
                    SecurityId = transactions[0].SecurityId
                };
                await context.Positions.AddAsync(cashPosition);

                // lock period
                AccountingPeriodsDIM period = context.Periods.Find(initialNav.NAVPeriodId);
                period.isLocked = true;
                context.Periods.Update(period);

                await context.InvestorHoldings.AddRangeAsync(investorHoldings);

                // Saves the fund investors that are attached to the transferagent subscriptions
                await context.FundInvestor.AddRangeAsync(fundInvestors);

                // Saves the first Nav
                await context.NavPriceData.AddAsync(initialNav);

                // saves the investors to the database
                context.TransferAgent.AddRange(investorSubscriptions);

                context.Transactions.AddRange(transactions);

                // Saves the funds state to initialised
                context.Funds.Update(fund);

                await context.SaveChangesAsync();
            }
        }

        public async Task LockNav(NavValuations navValuations)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                DateTime asOfDate = navValuations.AsOfDate;
                int fundId = navValuations.fund.FundId;
                AccountingPeriodsDIM period = context.Periods.Where(p => p.FundId == fundId && p.AccountingDate == asOfDate).FirstOrDefault();
                period.isLocked = true;

                context.Periods.Update(period);
                List<TransactionsBO> allTransactions;
                if (navValuations.fund.NAVFrequency == "Daily")
                {
                    allTransactions = navValuations.fund.Transactions.Where(t => t.TradeDate == asOfDate).ToList();
                }
                else
                {
                    allTransactions = navValuations.fund.Transactions.Where(t => t.TradeDate.Month == asOfDate.Month).ToList();
                }
                foreach (TransactionsBO transaction in allTransactions)
                {
                    transaction.isLocked = true;
                }
                context.Transactions.UpdateRange(allTransactions);

                NAVPriceStoreFACT newNavPrice = new NAVPriceStoreFACT
                {
                    FinalisedDate = asOfDate,
                    Currency = navValuations.fund.BaseCurrency,
                    FundId = fundId,
                    NAVPeriodId = period.PeriodId,
                    SharesOutstanding = navValuations.SharesOutstanding,
                    NetAssetValue = navValuations.NetAssetValue,
                    NAVPrice = navValuations.NetAssetValuePerShare,
                };
                context.NavPriceData.Add(newNavPrice);

                List<PositionFACT> newPositions = new List<PositionFACT>();
                foreach (ValuedSecurityPosition secPosition in navValuations.SecurityPositions)
                {
                    PositionFACT newPosition = new PositionFACT
                    {
                        PositionDate = secPosition.AsOfDate,
                        SecurityId = secPosition.Position.Security.SecurityId,
                        AssetClassId = secPosition.Position.Security.AssetClassId,
                        FundId = fundId,
                        AverageCost = secPosition.Position.AverageCost,
                        CurrencyId = secPosition.Position.Security.CurrencyId,
                        MarketValue = secPosition.MarketValueBase,
                        Price = secPosition.MarketPrice,
                        Quantity = secPosition.Position.NetQuantity,
                        RealisedPnl = secPosition.Position.RealisedPnL,
                        UnrealisedPnl = secPosition.UnrealisedPnl
                    };
                    newPositions.Add(newPosition);
                }
                foreach (ValuedCashPosition cashPosition in navValuations.CashPositions)
                {
                    string currencySecSymbol = $"{cashPosition.CashPosition.Currency.Symbol}c";
                    SecuritiesDIM securitisedCash = context.Securities.Where(s => s.Symbol == currencySecSymbol).Include(s => s.AssetClass).FirstOrDefault();

                    PositionFACT newPosition = new PositionFACT
                    {
                        PositionDate = cashPosition.AsOfDate,
                        SecurityId = securitisedCash.SecurityId,
                        AssetClassId = securitisedCash.AssetClassId,
                        FundId = fundId,
                        AverageCost = 1,
                        CurrencyId = cashPosition.CashPosition.Currency.CurrencyId,
                        MarketValue = cashPosition.MarketValueBase,
                        Price = cashPosition.fxRate,
                        Quantity = cashPosition.CashPosition.NetQuantity,
                        RealisedPnl = 0,
                        UnrealisedPnl = 0
                    };
                    newPositions.Add(newPosition);
                }

                await context.Positions.AddRangeAsync(newPositions);

                List<InvestorHoldingsFACT> newHoldings = new List<InvestorHoldingsFACT>();
                foreach (ClientHoldingValuation clientHolding in navValuations.ClientHoldings)
                {
                    InvestorHoldingsFACT newHolding = new InvestorHoldingsFACT
                    {
                        NetValuation = clientHolding.NetValuation,
                        AverageCost = clientHolding.Holding.AverageCost,
                        HighWaterMark = clientHolding.Holding.Investor.HighWaterMark,
                        ManagementFeesAccrued = clientHolding.ManagementFeesAccrued,
                        Units = clientHolding.Holding.Units,
                        PerformanceFeesAccrued = clientHolding.PerformanceFeesAccrued,
                        HoldingDate = asOfDate,
                        FundId = fundId,
                        InvestorId = clientHolding.Holding.Investor.InvestorId
                    };
                    newHoldings.Add(newHolding);
                }
                await context.InvestorHoldings.AddRangeAsync(newHoldings);



                await context.SaveChangesAsync();
                // Lock all transactions with this trade Date... DONE
                // Add to Position SnapShot Fact Table... DONE
                // Lock Period... DONE
                // Update TransferAgent Fact Table.... DONE
                // NavPrices DONE

            }
        }

        public async Task UnlockNav(DateTime asOfDate, int fundId)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {

                Fund fund = context.Funds.Find(fundId);
                AccountingPeriodsDIM period = context.Periods.Where(p => p.FundId == fund.FundId && p.AccountingDate == asOfDate).FirstOrDefault();
                period.isLocked = false;

                context.Periods.Update(period);

                List<TransactionsBO> allTransactions;
                if (fund.NAVFrequency == "Daily")
                {
                    allTransactions = context.Transactions.Where(t => t.FundId == fund.FundId && t.TradeDate == asOfDate).ToList();
                }
                else
                {
                    allTransactions = context.Transactions.Where(t => t.FundId == fund.FundId && t.TradeDate.Month == asOfDate.Month).ToList();
                }

                foreach (TransactionsBO transaction in allTransactions)
                {
                    transaction.isLocked = false;
                }
                context.Transactions.UpdateRange(allTransactions);

                NAVPriceStoreFACT navPrice = context.NavPriceData.Where(npd => npd.FinalisedDate == asOfDate && npd.FundId == fund.FundId).FirstOrDefault();
                context.NavPriceData.Remove(navPrice);

                IEnumerable<PositionFACT> storedPositions = context.Positions.Where(p => p.PositionDate == asOfDate && p.FundId == fund.FundId);
                context.Positions.RemoveRange(storedPositions);

                IEnumerable<InvestorHoldingsFACT> storedHoldings = context.InvestorHoldings.Where(i => i.HoldingDate == asOfDate && i.FundId == fund.FundId);
                context.InvestorHoldings.RemoveRange(storedHoldings);


                await context.SaveChangesAsync();
            }
        }

        public async Task<TransferAgencyBO> UpdateInvestorAction(TransferAgencyBO investorAction)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                context.TransferAgent.Update(investorAction);
                await context.SaveChangesAsync();

                return investorAction;
            }
        }
    }
}
