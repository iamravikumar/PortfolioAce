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
    public class FundService:IFundService
    {
        private readonly PortfolioAceDbContextFactory _contextFactory;

        public FundService(PortfolioAceDbContextFactory contextFactory)
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

        public bool FundExists(string fundSymbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return (context.Funds.FirstOrDefault(f => f.Symbol == fundSymbol) != null);
            }
        }

        public List<Fund> GetAllFunds()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                // include is having negative impact on performance
                // look for optimisation at some point
                return context.Funds
                    .Include(f=> f.NavPrices)
                    .Include(f=> f.TransferAgent)
                    .Include(f => f.Transactions)
                        .ThenInclude(s => s.Security)
                        .ThenInclude(s=>s.AssetClass)
                    .Include(f => f.Transactions)
                        .ThenInclude(c => c.Currency)
                    .Include(f => f.Transactions)
                        .ThenInclude(t => t.TransactionType)
                    .Include(f => f.Transactions)
                        .ThenInclude(cu => cu.Custodian)
                    .ToList();
            }
        }

        public Fund GetFund(string fundSymbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Funds.Where(f => f.Symbol == fundSymbol)
                            .Include(f=> f.NavPrices)
                            .Include(f => f.TransferAgent)
                            .Include(f => f.Transactions)
                                .ThenInclude(s => s.Security)
                                .ThenInclude(s => s.AssetClass)
                            .Include(f => f.Transactions)
                                .ThenInclude(c=>c.Currency)
                            .Include(f => f.Transactions)
                                .ThenInclude(t => t.TransactionType)
                            .Include(f=>f.Transactions)
                                .ThenInclude(cu=>cu.Custodian)
                            .FirstOrDefault();
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
