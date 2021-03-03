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
        Task AddSecurityInfo(SecuritiesDIM security);
        Task UpdateSecurityInfo(SecuritiesDIM security);
        Task DeleteSecurityInfo(string symbol);

        bool SecurityExists(string symbol);

        Task UpdateSecurityPrices(string symbol);
        List<SecuritiesDIM> GetAllSecurities();
    }
}
