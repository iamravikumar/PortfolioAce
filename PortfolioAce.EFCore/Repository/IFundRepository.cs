using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Repository
{
    public interface IFundRepository
    {
        Task<Fund> CreateFund(Fund fund);
        Task<Fund> UpdateFund(Fund fund);
        Task<Fund> DeleteFund(int id);
        bool FundExists(string fundSymbol, string fundName);
        List<Fund> GetAllFunds();
        Fund GetFund(string fundSymbol);
    }
}
