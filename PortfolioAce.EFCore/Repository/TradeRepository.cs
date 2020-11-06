using Microsoft.EntityFrameworkCore.ChangeTracking;
using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Repository
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
                EntityEntry<Trade> res = await context.Trades.AddAsync(trade);
                await context.SaveChangesAsync();

                return res.Entity;
            }
        }

        public async Task<Trade> DeleteTrade(int id)
        {
            throw new NotImplementedException();
        }

        public List<Trade> GetAllFundTrades(int fundId)
        {
            throw new NotImplementedException();
        }

        public async Task<Trade> GetCashTradeById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Trade> UpdateTrade(Trade trade)
        {
            throw new NotImplementedException();
        }
    }
}
