using PortfolioAce.DataCentre.DeserialisedObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services.PriceServices
{
    public interface IPriceService
    {
        Task<List<AVSecurityPriceData>> GetPrices(string symbol);
    }
}
