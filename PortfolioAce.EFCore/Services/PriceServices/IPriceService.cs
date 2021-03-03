using PortfolioAce.DataCentre.DeserialisedObjects;
using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services.PriceServices
{
    public interface IPriceService : IBaseService
    {
        Task<IEnumerable<AVSecurityPriceData>> AddDailyPrices(SecuritiesDIM security);
        Task AddManualPrices(List<SecurityPriceStore> prices);
        Task UpdateManualPrices(List<SecurityPriceStore> prices);

        SecuritiesDIM GetSecurityInfo(string symbol);

        List<SecurityPriceStore> GetSecurityPrices(string symbol);
    }
}
