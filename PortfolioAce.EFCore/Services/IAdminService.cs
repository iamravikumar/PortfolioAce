using PortfolioAce.Domain.Models.Dimensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public interface IAdminService : IBaseService
    {
        // Admin responsibilities. such as adding static security information
        // and prices. 
        // make these tasks
        Task<SecuritiesDIM> AddSecurityInfo(SecuritiesDIM security);
        Task UpdateSecurityInfo(SecuritiesDIM security);
        Task DeleteSecurityInfo(string symbol);

        bool SecurityExists(string symbol, string assetClass);

        Task UpdateSecurityPrices(string symbol);
        List<SecuritiesDIM> GetAllSecurities();
    }
}
