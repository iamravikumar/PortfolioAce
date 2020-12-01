using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PortfolioAce.Domain.Models.BackOfficeModels;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly PortfolioAceDbContextFactory _contextFactory;

        public TransactionService(PortfolioAceDbContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }

        public async Task<TransactionsBO> CreateTransaction(TransactionsBO transaction)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<TransactionsBO> res = await context.Transactions.AddAsync(transaction);
                await context.SaveChangesAsync();
                return res.Entity;
            }
        }

        public SecuritiesDIM GetSecurityInfo(string symbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Securities.Where(s => s.Symbol == symbol).Include(s=>s.Currency).FirstOrDefault();
            }
        }

        public TransactionTypeDIM GetTradeType(string name)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.TransactionTypes.Where(t => ((string)(object)t.TypeName) == name).FirstOrDefault();
            }
        }

        public bool SecurityExists(string symbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return (context.Securities.Where(s => s.Symbol == symbol).FirstOrDefault() != null);
            }
        }
    }
}
