using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using PortfolioAce.EFCore.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public class FundService : IFundService
    {
        private readonly PortfolioAceDbContextFactory _contextFactory;

        public FundService(PortfolioAceDbContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }

        public async Task<Fund> CreateFund(Fund fund)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<Fund> res = await context.Funds.AddAsync(fund);
                await context.SaveChangesAsync();
                // Once the fund has been created, I then create the accounting periods

                // set the monthly or daily account periods for up to one year ahead...
                DateTime startDate = fund.LaunchDate;
                List<DateTime> allPeriods;
                if (fund.NAVFrequency == "Daily")
                {
                    // get daily dates from fund launch to a year ahead
                    allPeriods = DateSettings.AnnualWorkingDays(startDate);
                }
                else
                {
                    // get month end dates from fund launch to a year ahead
                    allPeriods = DateSettings.AnnualMonthEnds(startDate);
                }
                // add all the dates to the new periods
                List<AccountingPeriodsDIM> initialPeriods = new List<AccountingPeriodsDIM>();
                foreach (DateTime period in allPeriods)
                {
                    AccountingPeriodsDIM newPeriod;
                    newPeriod = new AccountingPeriodsDIM { AccountingDate = period.Date, isLocked = false, FundId = fund.FundId };
                    initialPeriods.Add(newPeriod);
                }
                context.Periods.AddRange(initialPeriods);
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
                context.Funds.Remove(fund);
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
                    .Include(f => f.TransferAgent)
                        .ThenInclude(ta => ta.FundInvestor)
                        .ThenInclude(fi => fi.Investor)
                    .Include(f => f.Transactions)
                        .ThenInclude(s => s.Security)
                        .ThenInclude(s => s.AssetClass)
                    .Include(f => f.Transactions)
                        .ThenInclude(c => c.Currency)
                    .Include(f => f.Transactions)
                        .ThenInclude(t => t.TransactionType)
                    .Include(f => f.Transactions)
                        .ThenInclude(cu => cu.Custodian)
                    .Include(f => f.NavPrices)
                        .ThenInclude(nav => nav.NAVPeriod)
                    .ToList();
            }
        }

        public List<string> GetAllFundSymbols()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Funds.AsNoTracking().Select(f => f.Symbol).ToList();
            }
        }

        public Fund GetFund(string fundSymbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Funds.Where(f => f.Symbol == fundSymbol)
                            .Include(f => f.TransferAgent)
                                .ThenInclude(ta => ta.FundInvestor)
                                .ThenInclude(fi => fi.Investor)
                            .Include(f => f.Transactions)
                                .ThenInclude(s => s.Security)
                                .ThenInclude(s => s.AssetClass)
                            .Include(f => f.Transactions)
                                .ThenInclude(c => c.Currency)
                            .Include(f => f.Transactions)
                                .ThenInclude(t => t.TransactionType)
                            .Include(f => f.Transactions)
                                .ThenInclude(cu => cu.Custodian)
                            .Include(f => f.NavPrices)
                                .ThenInclude(nav => nav.NAVPeriod)
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
