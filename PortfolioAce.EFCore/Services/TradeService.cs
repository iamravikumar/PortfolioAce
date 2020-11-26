using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public class TradeService : ITradeService
    {
        private readonly PortfolioAceDbContextFactory _contextFactory;

        public TradeService(PortfolioAceDbContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }

        public async Task<TradeBO> CreateTrade(TradeBO trade)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                // I will need to refactor soon. This repo is fine but the VM should use a service layer then call this logic...
                // the service layer will contain the logic and validations as it relies on different repos.
                // check security database to see if security exists. if not it must be created first

                // trade amount in trades table should be negative for purchase and positive for sales.
                EntityEntry<TradeBO> res = await context.Trades.AddAsync(trade);
                await context.SaveChangesAsync();
                SecuritiesDIM security = context.Securities.Where(ts => ts.SecurityId==res.Entity.SecurityId).FirstOrDefault();
                CashBookBO transaction = TransactionMapper(res.Entity, new CashBookBO(), security);
                await context.CashBooks.AddAsync(transaction);
                await context.SaveChangesAsync();
                
                return res.Entity;
            }
        }

        public async Task<TradeBO> DeleteTrade(int id)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                TradeBO trade = await context.Trades.FindAsync(id);
                if (trade == null)
                {
                    return trade;
                }
                context.Trades.Remove(trade);
                //TODO: raise a warning if there is no transaction to remove. Big issue if this is the case.
                CashBookBO transaction = await context.CashBooks.Where(c => c.TradeId == id).FirstAsync();
                context.CashBooks.Remove(transaction);
                await context.SaveChangesAsync();

                return trade;
            }
        }

        public List<TradeBO> GetAllFundTrades(int fundId)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Trades.Where(c => c.FundId == fundId).OrderBy(c=>c.TradeDate).ToList();
            }
        }

        public SecuritiesDIM GetSecurityInfo(string symbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Securities.Where(s=> s.Symbol == symbol).FirstOrDefault();
            }
        }

        public async Task<TradeBO> GetTradeById(int id)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Trades.FindAsync(id);
            }
        }

        public List<TradeBO> GetTradesBySymbol(string symbol, int fundId)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Trades
                    .Where(t => t.FundId == fundId && t.Security.Symbol == symbol)
                    .OrderBy(t => t.TradeDate)
                    .ToList();
            }
        }

        public bool SecurityExists(string symbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return (context.Securities.Where(s => s.Symbol == symbol).FirstOrDefault() != null);
            }
        }

        public async Task<TradeBO> UpdateTrade(TradeBO trade)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                context.Trades.Update(trade);
                CashBookBO transaction = await context.CashBooks.Where(c => c.TradeId == trade.TradeId).FirstAsync();
                SecuritiesDIM security = context.Securities.Where(ts => ts.SecurityId == trade.SecurityId).FirstOrDefault();
                CashBookBO newTransaction = TransactionMapper(trade, transaction, security);
                context.CashBooks.Update(newTransaction);
                await context.SaveChangesAsync();

                return trade;
            }
        }

        private CashBookBO TransactionMapper(TradeBO trade, CashBookBO transaction, SecuritiesDIM security =null)
        {
            // maps trade information to a transaction in the database for the cashaccount.

            transaction.TradeId = trade.TradeId;
            transaction.TransactionType = trade.TradeTypeId;// trade or corp action
            transaction.TransactionAmount = trade.TradeAmount;
            transaction.TransactionDate = trade.SettleDate;
            transaction.Currency = trade.CurrencyId;
            transaction.FundId = trade.FundId;
            string comment;
            if (trade.TradeTypeId == "Corp Action")
            {
                comment = $"Corporate Action for {security.Symbol}";
            }
            else
            {
                comment = (trade.TradeAmount <= 0) ? $"BUY {security.Symbol}" : $"SELL {security.Symbol}";
            }
            transaction.Comment = comment;
            return transaction;
        }
    }
}
