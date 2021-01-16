﻿using PortfolioAce.DataCentre.DeserialisedObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services.PriceServices
{
    public interface IPriceService:IBaseService
    {
        Task<IEnumerable<AVSecurityPriceData>> AddDailyPrices(SecuritiesDIM security);
        void AddManualPrices(List<SecurityPriceStore> prices);
        void UpdateManualPrices(List<SecurityPriceStore> prices);

        List<string> GetAllSecuritySymbols();
        SecuritiesDIM GetSecurityInfo(string symbol);

        List<SecurityPriceStore> GetSecurityPrices(string symbol);
    }
}
