using PortfolioAce.DataCentre.DeserialisedObjects;
using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services.PriceServices
{
    public interface IPriceService
    {
        Task<List<AVSecurityPriceData>> AddPrices(string symbol);

    }
}
