using System;
using System.Collections.Generic;
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
    }
}
