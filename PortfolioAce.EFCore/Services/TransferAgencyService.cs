using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.Domain.Models.FactTables;
using PortfolioAce.EFCore.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<TransferAgencyBO> GetInvestorActionById(int id)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.TransferAgent.FindAsync(id);
            }
        }

        public void InitialiseFundAction(Fund fund, List<TransferAgencyBO> investorSubscriptions, List<TransactionsBO> transactions, NAVPriceStoreFACT initialNav, List<FundInvestorBO> fundInvestors)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                // I will also need to create positions on this too.. Cash Positions that is in the PositionFactTable

                // lock period
                AccountingPeriodsDIM period = context.Periods.Find(initialNav.NAVPeriodId);
                period.isLocked = true;
                context.Periods.Update(period);

                // Saves the fund investors that are attached to the transferagent subscriptions
                context.FundInvestor.AddRange(fundInvestors);

                // Saves the first Nav
                context.NavPriceData.Add(initialNav);


                // Saves the funds state to initialised
                context.Funds.Update(fund);

                
                // saves the investors to the database
                context.TransferAgent.AddRange(investorSubscriptions);

                context.Transactions.AddRange(transactions);


                context.SaveChanges();
            }
        }

        public void LockNav(NavValuations navValuations)
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
                foreach (SecurityPositionValuation secPosition in navValuations.SecurityPositions)
                {
                    PositionFACT newPosition = new PositionFACT
                    {
                        PositionDate = secPosition.AsOfDate,
                        SecurityId=secPosition.Position.security.SecurityId,
                        AssetClassId=secPosition.Position.security.AssetClassId,
                        FundId=fundId,
                        AverageCost=secPosition.Position.AverageCost,
                        CurrencyId=secPosition.Position.security.CurrencyId,
                        MarketValue=secPosition.MarketValueBase,
                        Price=secPosition.price,
                        Quantity=secPosition.Position.NetQuantity,
                        RealisedPnl=secPosition.Position.RealisedPnL,
                        UnrealisedPnl=secPosition.unrealisedPnl
                    };
                    newPositions.Add(newPosition);
                }
                foreach(CashPositionValuation cashPosition in navValuations.CashPositions)
                {
                    string currencySecSymbol = $"{cashPosition.CashPosition.currency.Symbol}c";
                    SecuritiesDIM securitisedCash = context.Securities.Where(s => s.Symbol == currencySecSymbol).Include(s => s.AssetClass).FirstOrDefault();

                    PositionFACT newPosition = new PositionFACT
                    {
                        PositionDate = cashPosition.AsOfDate,
                        SecurityId = securitisedCash.SecurityId,
                        AssetClassId = securitisedCash.AssetClassId,
                        FundId = fundId,
                        AverageCost = 1,
                        CurrencyId = cashPosition.CashPosition.currency.CurrencyId,
                        MarketValue = cashPosition.MarketValueBase,
                        Price = cashPosition.fxRate,
                        Quantity = cashPosition.CashPosition.AccountBalance,
                        RealisedPnl = 0,
                        UnrealisedPnl = 0
                    };
                    newPositions.Add(newPosition);
                }

                context.Positions.AddRange(newPositions);

                context.SaveChanges();
                // Lock all transactions with this trade Date... DONE
                // Add to Position SnapShot Fact Table... DONE
                // Lock Period... DONE
                // Update TransferAgent Fact Table I need to create this Table and implement the update....
                // NavPrices DONE

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
