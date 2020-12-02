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
    }
}
