using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioAce.EFCore.Services.SettingServices
{
    public class ImportService:IImportService
    {
        private readonly PortfolioAceDbContextFactory _contextFactory;

        public ImportService(PortfolioAceDbContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }

        public Dictionary<string, int> CurrencyMap()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Currencies.ToDictionary(k => k.Symbol, v => v.CurrencyId);
            }
        }
    }
}
