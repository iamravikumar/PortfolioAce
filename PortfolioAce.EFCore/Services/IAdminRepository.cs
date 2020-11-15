using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public interface IAdminRepository
    {
        // Admin responsibilities. such as adding static security information
        // and prices. 
        void AddSecurityInfo(Security security);
        void UpdateSecurityInfo(Security security);
        void DeleteSecurityInfo(string symbol);

        void UpdateSecurityPrices (string symbol);
    }
}
