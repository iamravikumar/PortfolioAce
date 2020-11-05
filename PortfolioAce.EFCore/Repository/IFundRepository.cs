using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Repository
{
    public interface IFundRepository
    {
        Task<Fund> GetById(int id);
        Task<Fund> Create(Fund fund);
        Task<Fund> Update(Fund fund);
        Task<Fund> Delete(int id);
        bool FundExists(string fundSymbol, string fundName);
        List<Fund> GetAllFunds();
        Fund GetFund(string fundName);
    }
}
