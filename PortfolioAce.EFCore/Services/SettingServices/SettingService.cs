using PortfolioAce.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioAce.EFCore.Services.SettingServices
{
    public class SettingService : ISettingService
    {
        private readonly PortfolioAceDbContextFactory _contextFactory;

        public SettingService(PortfolioAceDbContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }

        public Dictionary<string, ApplicationSettings> GetAllSettings()
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                return context.AppSettings.ToDictionary(s => s.SettingName);
            }
        }

        public void UpdateAPIKeys(string alphaVantageKeyValue, string FMPKeyValue)
        {
            using (PortfolioAceDbContext context = _contextFactory.CreateDbContext())
            {
                ApplicationSettings AvKey = context.AppSettings.Where(ap => ap.SettingName == "AlphaVantageAPI").First();
                ApplicationSettings FMPKey = context.AppSettings.Where(ap => ap.SettingName == "FMPrepAPI").First();

                AvKey.SettingValue = alphaVantageKeyValue;
                FMPKey.SettingValue = FMPKeyValue;

                context.Update(AvKey);
                context.Update(FMPKey);
                context.SaveChanges();
            }
        }
    }
}
