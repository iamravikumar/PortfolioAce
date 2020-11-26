using PortfolioAce.Domain.Models;
using PortfolioAce.Domain.Models.Dimensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public interface IAdminService
    {
        // Admin responsibilities. such as adding static security information
        // and prices. 
        // make these tasks
        void AddSecurityInfo(SecuritiesDIM security);
        void UpdateSecurityInfo(SecuritiesDIM security);
        void DeleteSecurityInfo(string symbol);

        bool SecurityExists(string symbol);

        void UpdateSecurityPrices (string symbol);
        List<SecuritiesDIM> GetAllSecurities();
    }
}
