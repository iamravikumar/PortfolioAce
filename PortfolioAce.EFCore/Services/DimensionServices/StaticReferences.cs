using Microsoft.EntityFrameworkCore;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return context.AssetClasses.ToList();
            }
        }

        public List<TransactionTypeDIM> GetAllTransactionTypes()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.TransactionTypes.ToList();
            }
        }

        public List<CurrenciesDIM> GetAllCurrencies()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Currencies.ToList();
            }
        }

        public List<IssueTypesDIM> GetAllIssueTypes()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.IssueTypes.ToList();
            }
        }

        public List<NavFrequencyDIM> GetAllNavFrequencies()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.NavFrequencies.ToList();
            }
        }

        public AssetClassDIM GetAssetClass(string name)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.AssetClasses.Where(a => ((string)(object)a.Name) == name).FirstOrDefault();
            }
        }

        public CurrenciesDIM GetCurrency(string symbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Currencies.Where(c => ((string)(object)c.Symbol) == symbol).FirstOrDefault();
            }
        }

        public TransactionTypeDIM GetTransactionType(string typeName)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.TransactionTypes.Where(c => ((string)(object)c.TypeName) == typeName).FirstOrDefault();
            }
        }

        public List<CustodiansDIM> GetAllCustodians()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Custodians.ToList();
            }
        }

        public CustodiansDIM GetCustodian(string custodianName)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Custodians.Where(c => ((string)(object)c.Name) == custodianName).FirstOrDefault();
            }
        }

        public SecuritiesDIM GetSecurityInfo(string symbol)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Securities.Where(s => s.Symbol == symbol).Include(s => s.Currency).Include(s => s.AssetClass).FirstOrDefault();
            }
        }

        public Dictionary<(SecuritiesDIM, DateTime), List<SecurityPriceStore>> GetAllSecurityPrices()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                List<SecurityPriceStore>  allPrices = context.SecurityPriceData.Include(s=>s.Security).ToList();
                Dictionary<(SecuritiesDIM, DateTime), List<SecurityPriceStore>> priceDict = new Dictionary<(SecuritiesDIM, DateTime), List<SecurityPriceStore>>();
                foreach(SecurityPriceStore price in allPrices)
                {
                    ValueTuple<SecuritiesDIM, DateTime> groupKey = (price.Security, price.Date);
                    if (!priceDict.ContainsKey(groupKey))
                    {
                        priceDict[groupKey] = new List<SecurityPriceStore> { price };
                    }
                    else
                    {
                        priceDict[groupKey].Add(price);
                    }
                }
                return priceDict;
            }
        }
    }
}
