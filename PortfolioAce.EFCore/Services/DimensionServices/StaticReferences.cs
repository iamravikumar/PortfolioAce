using Microsoft.EntityFrameworkCore;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioAce.EFCore.Services.DimensionServices
{
    public class StaticReferences : IStaticReferences
    {
        private readonly PortfolioAceDbContextFactory _contextFactory;

        public StaticReferences(PortfolioAceDbContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }

        public List<AssetClassDIM> GetAllAssetClasses()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.AssetClasses.AsNoTracking().ToList();
            }
        }

        public List<TransactionTypeDIM> GetAllTransactionTypes()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.TransactionTypes.AsNoTracking().ToList();
            }
        }

        public List<CurrenciesDIM> GetAllCurrencies()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Currencies.AsNoTracking().ToList();
            }
        }

        public List<IssueTypesDIM> GetAllIssueTypes()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.IssueTypes.AsNoTracking().ToList();
            }
        }

        public List<NavFrequencyDIM> GetAllNavFrequencies()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.NavFrequencies.AsNoTracking().ToList();
            }
        }

        public AssetClassDIM GetAssetClass(string name)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.AssetClasses.AsNoTracking().Where(a => (a.Name) == name).FirstOrDefault();
            }
        }

        public CurrenciesDIM GetCurrency(string symbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Currencies.AsNoTracking().Where(c => (c.Symbol) == symbol).FirstOrDefault();
            }
        }

        public TransactionTypeDIM GetTransactionType(string typeName)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.TransactionTypes.AsNoTracking().Where(c => (c.TypeName) == typeName).FirstOrDefault();
            }
        }

        public List<CustodiansDIM> GetAllCustodians()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Custodians.AsNoTracking().ToList();
            }
        }

        public CustodiansDIM GetCustodian(string custodianName)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Custodians.AsNoTracking().Where(c => (c.Name) == custodianName).FirstOrDefault();
            }
        }

        public SecuritiesDIM GetSecurityInfo(string symbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Securities.AsNoTracking().Where(s => s.Symbol == symbol).Include(s => s.Currency).Include(s => s.AssetClass).FirstOrDefault();
            }
        }

        public List<SecurityPriceStore> GetAllSecurityPrices(DateTime asOfDate)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.SecurityPriceData.Where(s => s.Date <= asOfDate).ToList();
            }
        }

        public Dictionary<(string, DateTime), decimal> GetPriceTable(DateTime asOfDate)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<SecurityPriceStore> allPrices = context.SecurityPriceData.AsNoTracking().Where(s => s.Date <= asOfDate).Include(s => s.Security);

                Dictionary<(string, DateTime), decimal> priceTable = new Dictionary<(string, DateTime), decimal>();
                foreach (SecurityPriceStore price in allPrices)
                {
                    ValueTuple<string, DateTime> groupKey = (price.Security.Symbol, price.Date); // this allows me to group prices by security AND date
                    priceTable[groupKey] = price.ClosePrice;
                }
                return priceTable;
            }
        }

        public AccountingPeriodsDIM GetPeriod(DateTime previousDate, int fundId)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Periods.AsNoTracking().Where(p => p.AccountingDate == previousDate && p.FundId == fundId).FirstOrDefault();
            }
        }

        public bool PreviousPeriodLocked(DateTime previousPeriodDate, int fundId)
        {
            // check if the previous period is locked or not
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                List<AccountingPeriodsDIM> prevPeriods = context.Periods.AsNoTracking().Where(p => p.AccountingDate < previousPeriodDate && p.FundId == fundId).OrderBy(p => p.AccountingDate).ToList();
                if (prevPeriods.Count == 0)
                {
                    return false;
                }
                else
                {
                    return (prevPeriods[prevPeriods.Count - 1].isLocked);
                }
            }
        }

        public DateTime GetMostRecentLockedDate(int fundId)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                // Defaults to today..
                AccountingPeriodsDIM periodDate = context.Periods.AsNoTracking().Where(p => p.FundId == fundId && p.isLocked).OrderBy(p => p.AccountingDate).LastOrDefault();
                return (periodDate != null) ? periodDate.AccountingDate : DateTime.Today;
            }
        }

        public List<AccountingPeriodsDIM> GetAllFundPeriods(int fundId)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Periods.AsNoTracking().Where(p => p.FundId == fundId).OrderBy(p => p.AccountingDate).ToList();
            }
        }

        public List<string> GetSecuritySymbolByAssetClass(string assetClass)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Securities.AsNoTracking().Where(s => s.AssetClass.Name == assetClass).OrderBy(s => s.Symbol).Select(s => s.Symbol).ToList();
            }
        }

        public List<SecuritiesDIM> GetAllSecurities(bool includeRates=false)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                if (includeRates)
                {
                    return context.Securities
                              .Where(s => s.AssetClass.Name != "Cash")
                              .Include(s => s.Currency)
                              .Include(s => s.AssetClass)
                              .ToList();
                }
                else
                {
                    return context.Securities
                          .Where(s => s.AssetClass.Name != "FX" && s.AssetClass.Name != "Cash" && s.AssetClass.Name != "InterestRate" && s.AssetClass.Name != "FXForward")
                          .Include(s => s.Currency)
                          .Include(s => s.AssetClass)
                          .ToList();
                }

            }
        }

        public List<Fund> GetAllFundsReference()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Funds.AsNoTracking().Where(f=>f.IsInitialised).ToList();
            }
        }
    }
}
