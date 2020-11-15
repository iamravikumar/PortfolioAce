using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public class TradeRepository : ITradeRepository
    {
        private readonly PortfolioAceDbContextFactory _contextFactory;

        public TradeRepository(PortfolioAceDbContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }

        public async Task<Trade> CreateTrade(Trade trade)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                // I will need to refactor soon. This repo is fine but the VM should use a service layer then call this logic...
                // the service layer will contain the logic and validations as it relies on different repos.
                // check security database to see if security exists. if not it must be created first

                // trade amount in trades table should be negative for purchase and positive for sales.
                EntityEntry<Trade> res = await context.Trades.AddAsync(trade);
                await context.SaveChangesAsync();
                CashBook transaction = TransactionMapper(res.Entity, new CashBook());
                await context.CashBooks.AddAsync(transaction);
                await context.SaveChangesAsync();
                
                return res.Entity;
            }
        }

        public async Task<Trade> DeleteTrade(int id)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                Trade trade = await context.Trades.FindAsync(id);
                if (trade == null)
                {
                    return trade;
                }
                context.Trades.Remove(trade);
                //TODO: raise a warning if there is no transaction to remove. Big issue if this is the case.
                CashBook transaction = await context.CashBooks.Where(c => c.TradeId == id).FirstAsync();
                context.CashBooks.Remove(transaction);
                await context.SaveChangesAsync();

                return trade;
            }
        }

        public List<Trade> GetAllFundTrades(int fundId)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Trades.Where(c => c.FundId == fundId).OrderBy(c=>c.TradeDate).ToList();
            }
        }

        public async Task<Trade> GetTradeById(int id)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Trades.FindAsync(id);
            }
        }

        public List<Trade> GetTradesBySymbol(string symbol, int fundId)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Trades
                    .Where(t => t.FundId == fundId && t.Symbol == symbol)
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

        public async Task<Trade> UpdateTrade(Trade trade)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                context.Trades.Update(trade);
                CashBook transaction = await context.CashBooks.Where(c => c.TradeId == trade.TradeId).FirstAsync();
                CashBook newTransaction = TransactionMapper(trade, transaction);
                context.CashBooks.Update(newTransaction);
                await context.SaveChangesAsync();

                return trade;
            }
        }

        private CashBook TransactionMapper(Trade trade, CashBook transaction)
        {
            // maps trade information to a transaction in the database for the cashaccount.

            transaction.TradeId = trade.TradeId;
            transaction.TransactionType = trade.TradeType;// trade or corp action
            transaction.TransactionAmount = trade.TradeAmount;
            transaction.TransactionDate = trade.SettleDate;
            transaction.Currency = trade.Currency;
            transaction.FundId = trade.FundId;
            string comment;
            if (trade.TradeType=="Corp Action")
            {
                comment = $"Corporate Action for {trade.Symbol}";
            }
            else
            {
                comment = (trade.TradeAmount <= 0) ? $"BUY {trade.Symbol}" : $"SELL {trade.Symbol}";
            }
            transaction.Comment = comment;
            return transaction;
        }
    }
}
