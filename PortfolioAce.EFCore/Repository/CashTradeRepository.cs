using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PortfolioAce.EFCore.Repository
{
    public class CashTradeRepository : ICashTradeRepository
    {
        private readonly PortfolioAceDbContextFactory _contextFactory;

        public CashTradeRepository(PortfolioAceDbContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }
        public async Task<CashTrade> CreateCashTrade(CashTrade cashTrade)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<CashTrade> res = await context.CashTrades.AddAsync(cashTrade);
                await context.SaveChangesAsync();
                CashBook transaction = TransactionMapper(res.Entity, new CashBook());
                await context.CashBooks.AddAsync(transaction);
                await context.SaveChangesAsync();

                return res.Entity;
            }
        }

        public async Task<CashTrade> DeleteCashTrade(int id)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                CashTrade cashTrade = await context.CashTrades.FindAsync(id);
                if (cashTrade == null)
                {
                    return cashTrade;
                }
                context.CashTrades.Remove(cashTrade);
                //TODO: raise a warning if there is no transaction to remove. Big issue if this is the case.
                CashBook transaction = await context.CashBooks.Where(c => c.CashTradeId == id).FirstAsync();
                context.CashBooks.Remove(transaction);
                await context.SaveChangesAsync();
                
                return cashTrade;
            }
        }

        public List<CashTrade> GetAllFundCashTrades(int fundId)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.CashTrades.Where(c => c.FundId == fundId).OrderBy(c=>c.TradeDate).ToList();
            }
        }

        public async Task<CashTrade> GetCashTradeById(int id)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.CashTrades.FindAsync(id);
            }
        }

        public async Task<CashTrade> UpdateCashTrade(CashTrade cashTrade)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                context.CashTrades.Update(cashTrade);
                CashBook transaction = await context.CashBooks.Where(c=>c.CashTradeId ==cashTrade.CashTradeId).FirstAsync();
                CashBook newTransaction = TransactionMapper(cashTrade, transaction);
                context.CashBooks.Update(newTransaction);
                await context.SaveChangesAsync();

                return cashTrade;
            }
        }

        private CashBook TransactionMapper(CashTrade cashTrade, CashBook transaction)
        {
            // maps cash trade information to a transaction in the database.
            
            transaction.CashTradeId = cashTrade.CashTradeId;
            transaction.TransactionType = cashTrade.CashType;
            decimal amount = cashTrade.Amount;

            transaction.TransactionAmount = (cashTrade.CashType == "Expense")?-amount:amount;
            transaction.TransactionDate = cashTrade.SettleDate;
            transaction.Currency = cashTrade.Currency;
            transaction.FundId = cashTrade.FundId;
            transaction.Comment = cashTrade.Comment;
            return transaction;
        }
    }
}
