using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Services
{
    public interface IFundService
    {
        Task<Fund> CreateFund(Fund fund);
        Task<Fund> UpdateFund(Fund fund);
        Task<Fund> DeleteFund(int id);
        bool FundExists(string fundSymbol);
        List<Fund> GetAllFunds();
        Fund GetFund(string fundSymbol);
    }
}
