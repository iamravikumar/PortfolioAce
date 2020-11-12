using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Repository
{
    public class FundRepository:IFundRepository
    {
        private readonly PortfolioAceDbContextFactory _contextFactory;

        public FundRepository(PortfolioAceDbContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }

        public async Task<Fund> CreateFund(Fund fund)
        {
            using(PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<Fund> res = await context.Set<Fund>().AddAsync(fund);
                await context.SaveChangesAsync();
                
                return res.Entity;
            }
        }

        public async Task<Fund> DeleteFund(int id)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                Fund fund = await context.Funds.FindAsync(id);
                if (fund == null)
                {
                    return fund;
                }
                context.Set<Fund>().Remove(fund);
                await context.SaveChangesAsync();

                return fund;
            }
        }

        public bool FundExists(string fundSymbol, string fundName)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return (context.Funds.FirstOrDefault(f => f.FundName == fundName || f.Symbol == fundSymbol) != null);
            }
        }

        public List<Fund> GetAllFunds()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Funds
                    .Include(f => f.CashBooks)
                    .Include(f => f.Trades)
                    .ToList();
            }
        }

        public Fund GetFund(string fundSymbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Funds.Where(f => f.Symbol == fundSymbol)
                            .Include(f => f.CashBooks)
                            .Include(f => f.Trades)
                            .FirstOrDefault();

                // the group by code etc can be used for when i actually need it.
                /*
                return context.Funds.Where(f=>f.Symbol==fundSymbol)
                    .Include(f => f.CashBooks.GroupBy(c => c.Currency)
                                                .Select(g => new {
                                                    g.Key,
                                                    ccyTotal = g.Sum(s => s.TransactionAmount)
                                                }))
                    .Include(f => f.Trades.OrderBy(t => t.TradeDate))
                    .FirstOrDefault();
                */
            }
        }


        public async Task<Fund> UpdateFund(Fund fund)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                context.Funds.Update(fund);

                await context.SaveChangesAsync();

                return fund;
            }
        }
    }
}
