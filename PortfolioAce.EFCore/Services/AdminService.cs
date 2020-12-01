﻿using Microsoft.EntityFrameworkCore;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void AddSecurityInfo(SecuritiesDIM security)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                context.Securities.Add(security);
                context.SaveChangesAsync();
            }
        }

        public void DeleteSecurityInfo(string symbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                SecuritiesDIM security = context.Securities.Where(s => s.Symbol == symbol).FirstOrDefault();
                context.Securities.Remove(security);
                context.SaveChangesAsync();
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
                return (context.Securities.FirstOrDefault(s=>s.Symbol == symbol) != null);
            }
        }

        public void UpdateSecurityInfo(SecuritiesDIM security)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                context.Securities.Update(security);
                context.SaveChangesAsync();
            }
        }

        public void UpdateSecurityPrices(string symbol)
        {
            throw new NotImplementedException();
        }
    }
}
