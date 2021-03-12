using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services.SettingServices
{
    public class ImportService:IImportService
    {
        private readonly PortfolioAceDbContextFactory _contextFactory;

        public ImportService(PortfolioAceDbContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }

        public async Task AddImportedPrices(Dictionary<string, List<SecurityPriceStore>> newPrices)
        {
            /* Use the symbol (Key) to get a hashset of all the currently saved dates.
             * get all the prices from the list (Value) and check if they already exist in the hashset.
             * if they dont then add them.
             */
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                foreach(KeyValuePair<string, List<SecurityPriceStore>> kvp in newPrices)
                {
                    HashSet<DateTime> existingDates = context.SecurityPriceData.Where(spd => spd.Security.Symbol == kvp.Key).Select(spd => spd.Date).ToHashSet();
                    foreach(SecurityPriceStore price in kvp.Value)
                    {
                        if (!existingDates.Contains(price.Date))
                        {
                            existingDates.Add(price.Date);
                            context.SecurityPriceData.Add(price);
                        }
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task AddImportedSecurities(List<SecuritiesDIM> newSecurities)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                HashSet<string> existingSecurities = context.Securities.Select(s => s.Symbol).ToHashSet();
                foreach(SecuritiesDIM security in newSecurities)
                {
                    if (!existingSecurities.Contains(security.Symbol))
                    {
                        existingSecurities.Add(security.Symbol);
                        context.Securities.Add(security);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public Dictionary<string, int> AssetClassMap()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.AssetClasses.ToDictionary(k => k.Name, v => v.AssetClassId);
            }
        }

        public Dictionary<string, int> CurrencyMap()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Currencies.ToDictionary(k => k.Symbol, v => v.CurrencyId);
            }
        }

        public Dictionary<string, int> SecurityMap()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Securities.ToDictionary(k => k.Symbol, v => v.SecurityId);
            }
        }
    }
}
