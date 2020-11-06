using PortfolioAce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Repository
{
    public interface ITradeRepository
    {
        Task<Trade> GetCashTradeById(int id);
        Task<Trade> CreateTrade(Trade trade);
        Task<Trade> UpdateTrade(Trade trade);
        Task<Trade> DeleteTrade(int id);
        List<Trade> GetAllFundTrades(int fundId);

    }
}
