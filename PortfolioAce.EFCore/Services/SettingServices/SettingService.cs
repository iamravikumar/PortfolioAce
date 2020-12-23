using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioAce.EFCore.Services.SettingServices
{
    public class SettingService:ISettingService
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
    }
}
