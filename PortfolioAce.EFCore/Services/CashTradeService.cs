using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.BackOfficeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PortfolioAce.EFCore.Services
{
    public class CashTradeService : ICashTradeService
    {
        private readonly PortfolioAceDbContextFactory _contextFactory;

        public CashTradeService(PortfolioAceDbContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }
        public async Task<CashTradeBO> CreateCashTrade(CashTradeBO cashTrade)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<CashTradeBO> res = await context.CashTrades.AddAsync(cashTrade);
                await context.SaveChangesAsync();
                CashBookBO transaction = TransactionMapper(res.Entity, new CashBookBO());
                await context.CashBooks.AddAsync(transaction);
                await context.SaveChangesAsync();

                return res.Entity;
            }
        }

        public async Task<CashTradeBO> DeleteCashTrade(int id)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                CashTradeBO cashTrade = await context.CashTrades.FindAsync(id);
                if (cashTrade == null)
                {
                    return cashTrade;
                }
                context.CashTrades.Remove(cashTrade);
                //TODO: raise a warning if there is no transaction to remove. Big issue if this is the case.
                CashBookBO transaction = await context.CashBooks.Where(c => c.CashTradeId == id).FirstAsync();
                context.CashBooks.Remove(transaction);
                await context.SaveChangesAsync();
                
                return cashTrade;
            }
        }

        public List<CashTradeBO> GetAllFundCashTrades(int fundId)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.CashTrades.Where(c => c.FundId == fundId).OrderBy(c=>c.TradeDate).ToList();
            }
        }

        public async Task<CashTradeBO> GetCashTradeById(int id)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.CashTrades.FindAsync(id);
            }
        }

        public async Task<CashTradeBO> UpdateCashTrade(CashTradeBO cashTrade)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                context.CashTrades.Update(cashTrade);
                CashBookBO transaction = await context.CashBooks.Where(c=>c.CashTradeId ==cashTrade.CashTradeId).FirstAsync();
                CashBookBO newTransaction = TransactionMapper(cashTrade, transaction);
                context.CashBooks.Update(newTransaction);
                await context.SaveChangesAsync();

                return cashTrade;
            }
        }

        private CashBookBO TransactionMapper(CashTradeBO cashTrade, CashBookBO transaction)
        {
            // maps cash trade information to a transaction in the database.
            
            transaction.CashTradeId = cashTrade.CashTradeId;
            transaction.TransactionType = cashTrade.CashTradeType;
            decimal amount = cashTrade.Amount;

            transaction.TransactionAmount = (cashTrade.CashTradeType == "Expense")?-amount:amount;
            transaction.TransactionDate = cashTrade.SettleDate;
            transaction.Currency = cashTrade.Currency;
            transaction.FundId = cashTrade.FundId;
            transaction.Comment = cashTrade.Comment;
            return transaction;
        }
    }
}
