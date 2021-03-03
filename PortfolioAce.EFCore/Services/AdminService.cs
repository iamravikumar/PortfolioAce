using Microsoft.EntityFrameworkCore;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public class AdminService : IAdminService
    {
        private readonly PortfolioAceDbContextFactory _contextFactory;

        public AdminService(PortfolioAceDbContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }

        public async Task AddSecurityInfo(SecuritiesDIM security)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                context.Securities.Add(security);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteSecurityInfo(string symbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                SecuritiesDIM security = context.Securities.Where(s => s.Symbol == symbol).FirstOrDefault();
                context.Securities.Remove(security);
                await context.SaveChangesAsync();
            }
        }

        public List<SecuritiesDIM> GetAllSecurities()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Securities
                        .Include(s => s.AssetClass)
                        .Include(s => s.Currency)
                        .OrderBy(s => s.SecurityName).ToList(); ;
            }
        }

        public bool SecurityExists(string symbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return (context.Securities.FirstOrDefault(s => s.Symbol == symbol) != null);
            }
        }

        public async Task UpdateSecurityInfo(SecuritiesDIM security)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                context.Securities.Update(security);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateSecurityPrices(string symbol)
        {
            throw new NotImplementedException();
        }
    }
}
